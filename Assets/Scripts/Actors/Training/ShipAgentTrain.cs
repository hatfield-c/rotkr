using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ShipAgentTrain : ShipAgent
{
    [Header("Train Parameters")]
    public float minDistance;
    public float desiredDistance;
    public float distancePadding;
    public float minSpeed;

    protected bool hasCollided = false;

    void FixedUpdate(){
        this.hasCollided = false;
        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

        if (distance <= this.minDistance) {
            this.SetReward(RewardParameters.PUNISH_PlayerCollide);
            this.resetFunction();
            return;
        }
        
       if(distance < this.desiredDistance - this.distancePadding) {
            this.AddReward(RewardParameters.PUNISH_TooClose);
        } else if(distance >= this.desiredDistance - this.distancePadding && distance <= this.desiredDistance + this.distancePadding) {
            this.AddReward(RewardParameters.REWARD_Proximity);
        } else{
            this.AddReward(RewardParameters.PUNISH_Inaction);
        }
        
    }

    void OnCollisionEnter(Collision collision){
        if (this.hasCollided) {
            return;
        }

        this.hasCollided = true;
        string tag = collision.gameObject.tag;

        if(tag == "terrain"){
            this.SetReward(RewardParameters.PUNISH_TerrainCollide);
            this.resetFunction();
            return;
        }
    }

    public override void OnActionReceived(float[] vectorAction) {
        base.OnActionReceived(vectorAction);

        
        int action0 = Mathf.FloorToInt(vectorAction[0]);

        if (action0 == 1) {
            float angle = Vector3.SignedAngle(Vector3.forward, this.vectorBuffer.normalized, Vector3.up);

            float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

            //if (Mathf.Abs(angle) < 90f && distance > this.minDistance + this.distancePadding) {
            if (distance > this.minDistance + this.distancePadding) {
                this.AddReward(RewardParameters.REWARD_Movement);
            }
        } else if(action0 == 3) {
            float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

            if (distance >= this.desiredDistance - this.distancePadding && distance <= this.desiredDistance + this.distancePadding) {
                this.AddReward(RewardParameters.REWARD_Movement);
            }
        }

    }

    public override void OnEpisodeBegin(){

    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;

        if (Input.GetKey(KeyCode.W)){
            actionsOut[0] = 1f;
        } else if(Input.GetKey(KeyCode.S)){
            actionsOut[0] = 2f;
        }

        if (Input.GetKey(KeyCode.A)){
            actionsOut[1] = 2f;
        } else if(Input.GetKey(KeyCode.D)){
            actionsOut[1] = 1f;
        }
    }


}
