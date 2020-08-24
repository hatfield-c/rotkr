using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ShipAgentTrain : ShipAgent
{
    public delegate void ResetFunction();

    [Header("Train Parameters")]
    public float minDistance;
    public float maxDistance;

    [Header("Blackboard")]
    public float minDistPunish;
    public float maxDistPunish;
    public ResetFunction resetFunction;

    void FixedUpdate(){
        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

        if(distance <= this.minDistance){
            this.AddReward(minDistPunish);
        } else if(distance >= this.maxDistance){
            this.AddReward(maxDistPunish);
        }
    }

    void OnCollisionEnter(Collision collision){
        string tag = collision.gameObject.tag;

        if(tag == "terrain"){
            this.AddReward(RewardParameters.PUNISH_TerrainCollide);
            this.resetFunction();
            return;
        }
        
        if(tag == "player"){
            this.AddReward(RewardParameters.PUNISH_PlayerCollide);
            this.resetFunction();
            return;
        }
    }

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
