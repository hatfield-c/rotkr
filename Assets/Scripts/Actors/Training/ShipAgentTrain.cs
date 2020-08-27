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

    protected bool hasCollided = false;

    void FixedUpdate(){
        this.hasCollided = false;
        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

        if (distance <= this.minDistance) {
            this.AddReward(RewardParameters.PUNISH_MinDistance);
        } else if (distance >= this.maxDistance) {
            this.AddReward(RewardParameters.PUNISH_MaxDistance);
        } else {
            this.AddReward(RewardParameters.PUNISH_Frame);
        }

        this.AddReward(RewardParameters.PUNISH_Rotation * (this.shipBody.angularVelocity.y / this.MaxAngularSpeed));
    }

    void OnCollisionEnter(Collision collision){
        if (this.hasCollided) {
            return;
        }

        this.hasCollided = true;
        string tag = collision.gameObject.tag;

        if(tag == "Player" || tag == "ship_deck"){
            this.AddReward(RewardParameters.GetPlayerPunishment(TrainAcademy.TIME_Elapsed));
            this.resetFunction();
            return;
        }

        if(tag == "terrain"){
            this.AddReward(RewardParameters.GetTerrainPunishment(TrainAcademy.TIME_Elapsed));
            this.resetFunction();
            return;
        }
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
            actionsOut[0] = 3f;
        }

        if(Input.GetKey(KeyCode.A)){
            actionsOut[1] = 3f;
        } else if(Input.GetKey(KeyCode.D)){
            actionsOut[1] = 1f;
        }

        if(Input.GetKey(KeyCode.Space)){
            actionsOut[2] = 1f;   
        }
    }


}
