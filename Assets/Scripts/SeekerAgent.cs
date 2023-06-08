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
    public float rayLength;
    private bool _hasFoundHider;
    public override void OnEpisodeBegin() 
    {
        _rigidbody ??= GetComponentInChildren<Rigidbody>();
        _rigidbody.velocity = Vector3.zero;
        _hiders ??= GameObject.FindGameObjectsWithTag("Hider").ToArray();
        transform.localPosition = new Vector3(-40f, 0, 2.4f);
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

        _hasFoundHider = false;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
       // base.CollectObservations(sensor);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.0001f);
        MoveAgent(actions.DiscreteActions);
        /*   AddReward(-0.0002f);
           var fwd = actions.ContinuousActions[0];
           var strafe = actions.ContinuousActions[1];//go forward and backwards
           var rotation = actions.ContinuousActions[2]; //turn left or right


           // Make agent move forward or backward
           transform.localPosition += transform.forward * fwd * 8f * Time.deltaTime;

           // Make agent strafe left or right
           transform.localPosition += transform.right * strafe * 8f * Time.deltaTime;

           // Make agent rotate
           var rotationAngle = rotation * 10f;
           transform.Rotate(transform.up, rotationAngle);

           ///
           //make agent move forward or backward
           //transform.forward = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + fwd * 10f * Time.deltaTime);
         //  transform.localPosition +=  Math.Max(fwd,0) * 8f * Time.deltaTime*transform.forward;
           //make agent rotate*/

        //var ray = new Ray(this.transform.position, this.transform.forward);
        //if (Physics.Raycast(ray, out RaycastHit hit,rayLength))
       // {
         //   if (hit.transform.gameObject.CompareTag("Hider") && !_hasFoundHider)
          //  {                
           //     var duration = DateTime.Now - _startTime;
            //    Debug.Log($"found object in: {duration.Minutes} minutes and {duration.Seconds} seconds");
               // AddReward(0.25f);
             //   _hasFoundHider = true;
            //}
        //}
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }

    }
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * -1f;
                break;
            case 2:
                dirToGo = transform.forward * 1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        _rigidbody.AddForce(dirToGo, ForceMode.VelocityChange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hider"))
        {
            AddReward(2f);
            EndEpisode();
        }
    }
}

