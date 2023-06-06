using System;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SeekerAgentPhilippe : Agent // mlagents-learn config/SeekerAgent.yaml --run-id=SeekerAgent
{
    private GameObject[] _hiders;
    private int _lastIndex;
    private Rigidbody _rigidbody;
    private float startTime;

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
        Debug.Log(_hiders.Count());
        Debug.Log(currentIndex);
        _hiders[currentIndex - 1].SetActive(true);

        startTime = Time.time;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //AddReward(-0.0002f);
        var fwd = actions.ContinuousActions[0]; //go forward and backwards
        var rotation = actions.ContinuousActions[1]; //turn left or right

        //make agent move forward or backward
        transform.localPosition += Math.Max(fwd, 0) * 8f * Time.deltaTime * transform.forward;

        // Calculate the rotation angle based on the rotation input
        var rotationAngle = rotation * 10f;

        // Rotate the agent around its own vertical axis
        transform.Rotate(transform.up, rotationAngle);

        var ray = new Ray(this.transform.position, this.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 70f))
        {
            if (hit.transform.gameObject.CompareTag("Hider"))
            {
                float timeTaken = Time.time - startTime;
                AddReward(0.5f);
                if (timeTaken <= 60f)
                {
                    AddReward(0.5f);
                }
                else if (timeTaken <= 120f)
                {
                    AddReward(0.3f);
                }
                else if (timeTaken <= 180)
                {
                    AddReward(0.15f);
                }
                else
                {
                    EndEpisode();
                }
                EndEpisode();
            }
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hider"))
        {
            AddReward(1f);
            EndEpisode();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            // AddReward(-0.1f);
            // EndEpisode();
        }
    }

    public void ZoneReward()
    {
        AddReward(0.05f);
    }
}
