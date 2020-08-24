using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ShipAgentTrain : ShipAgent
{

    [Header("Blackboard")]
    public float minDistPunish;
    public float maxDistPunish;

    public void ResetAgent(){
        Debug.Log("Agent Reset");

        this.shipBody.velocity = Vector3.zero;
        this.shipBody.angularVelocity = Vector3.zero;
        this.transform.eulerAngles = new Vector3(
            0,
            Random.Range(0f, 360f),
            0
        );

        this.shipManager.DisableSubsystems();
        this.shipManager.EnableSubsystems();
    }
 
    //***
    //***   vectorAction:
    //***       0 : Acceleration
    //***       1 : Turn Direction
    //***       2 : Shoot
    //***
    public override void OnActionReceived(float[] vectorAction){

        ShipAgentActions actions = new ShipAgentActions(
            vectorAction[0],
            vectorAction[1],
            vectorAction[2]
        );

        this.shipManager.TakeAction(actions);
    }

    public override void OnEpisodeBegin(){
        Debug.Log("Episode begin.");
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(0);
    }

    public override void Heuristic(float[] actionsOut){

    }

    public void EmptyReset() {}
}
