using System;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SeekerAgent : Agent // mlagents-learn config/SeekerAgent.yaml --run-id=SeekerAgent
{
    private GameObject[] _hiders;
    private DateTime _startTime;
    private int _lastIndex;
    private Rigidbody _rigidbody;
    public override void OnEpisodeBegin() 
    {
        _rigidbody ??= GetComponentInChildren<Rigidbody>();
        _hiders ??= GameObject.FindGameObjectsWithTag("Hider").ToArray();
        transform.localPosition = new Vector3(-37.9f, 0, 0.2f);
        _hiders.ToList().ForEach(h => h.SetActive(false));
        int currentIndex = _lastIndex;
        while (currentIndex == _lastIndex)
        {
            currentIndex = UnityEngine.Random.Range(0, _hiders.Count()+1);
        }
        Debug.Log(_hiders.Count());
        Debug.Log(currentIndex);
        _hiders[currentIndex-1].SetActive(true);

        _startTime = DateTime.Now;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
       // base.CollectObservations(sensor);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.0002f);
        var fwd = actions.ContinuousActions[0]; //go forward and backwards
        var rotation = actions.ContinuousActions[1]; //turn left or right

        //make agent move forward or backward
        //transform.forward = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + fwd * 10f * Time.deltaTime);
        transform.localPosition +=  Math.Max(fwd,0) * 8f * Time.deltaTime*transform.forward;
        //make agent rotate
        transform.Rotate(transform.up * rotation * 10f);
        

        var ray = new Ray(this.transform.position, this.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit,70f))
        {
            if (hit.transform.gameObject.CompareTag("Hider"))
            {                
                var duration = DateTime.Now - _startTime;
                Debug.Log($"found object in: {duration.Minutes} minutes and {duration.Seconds} seconds");
                AddReward(1);
               // AddReward(1f - (float)(duration.TotalSeconds/TimeSpan.FromMinutes(2).TotalSeconds));
                EndEpisode();
            }
        }
        //if((DateTime.Now - _startTime).TotalMinutes >= 1)
        //{
         //   AddReward(-0.5f);
         //   Debug.Log("Times up!");
         //   transform.position = new Vector3(0, 0, 0);
        //    EndEpisode();
       // }
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
          //  AddReward(-1.0f);
          //  EndEpisode();
        }
    }
}
