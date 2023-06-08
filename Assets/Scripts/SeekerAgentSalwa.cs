using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SeekerAgentSalwa : Agent // mlagents-learn config/SeekerAgent.yaml --run-id=SeekerAgent2
{
    private GameObject[] _hiders;
    private int _lastIndex;
    private Rigidbody _rigidbody;
    private DateTime startTime;


    //rewards
    //hider vindt
    //negatieve reward
    //botst tegen muren 
    public override void OnEpisodeBegin()
    {
        _rigidbody ??= GetComponentInChildren<Rigidbody>();
        _hiders ??= GameObject.FindGameObjectsWithTag("Hider").ToArray();
        transform.localPosition = new Vector3(-37.9f, 0, 0.2f);
        _hiders.ToList().ForEach(h => h.SetActive(false));
        int currentIndex = _lastIndex;
        while (currentIndex == _lastIndex)
        {
            currentIndex = UnityEngine.Random.Range(0, _hiders.Count() + 1);
        }
        _hiders[currentIndex - 1].SetActive(true);

        startTime = DateTime.Now;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {

        var fwd = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1; //go forward and backwards
        var rotation = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1; //turn left or right

        //make agent move forward or backward
        transform.localPosition += Math.Max(fwd, 0) * 8f * Time.deltaTime * transform.forward;

        // Calculate the rotation angle based on the rotation input
        var rotationAngle = rotation * 10f;

        // Rotate the agent around its own vertical axis
        transform.Rotate(transform.up, rotationAngle);

        
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            AddReward(-0.5f);
        if (collision.gameObject.CompareTag("Hider"))
        {
            AddReward(1f);
            //collision.gameObject.SetActive(false);
            Debug.Log("Found hider in " + (startTime - DateTime.Now));
            EndEpisode();
        }


    }
}

