using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ShipAgentTrain : ShipAgent
{
    [Header("Train Parameters")]
    public float minDistance;
    public float maxDistance;

    void FixedUpdate(){
        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

        if(distance <= this.minDistance){
            this.AddReward(RewardParameters.PUNISH_MinDistance);
        } else if(distance >= this.maxDistance){
            this.AddReward(RewardParameters.PUNISH_MaxDistance);
        }
    }

    void OnCollisionEnter(Collision collision){
        string tag = collision.gameObject.tag;

        if(tag == "Player" || tag == "ship_deck"){
            this.AddReward(RewardParameters.GetTerrainPunishment(TrainAcademy.TIME_Elapsed));
            this.resetFunction();
            return;
        }

        if(tag == "terrain"){
            this.AddReward(RewardParameters.GetPlayerPunishment(TrainAcademy.TIME_Elapsed));
            this.resetFunction();
            return;
        }
    }

    public void ResetAgent(){
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

    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[2] = 0f;

        if(Input.GetKey(KeyCode.W)){
            actionsOut[0] = 1f;
        } else if(Input.GetKey(KeyCode.S)){
            actionsOut[0] = -1f;
        }

        if(Input.GetKey(KeyCode.A)){
            actionsOut[1] = -1f;
        } else if(Input.GetKey(KeyCode.D)){
            actionsOut[1] = 1f;
        }

        if(Input.GetKey(KeyCode.Space)){
            actionsOut[2] = 0.75f;   
        }
    }


}
