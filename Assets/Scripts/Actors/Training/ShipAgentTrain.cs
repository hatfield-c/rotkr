using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ShipAgentTrain : ShipAgent
{
    [Header("Train Parameters")]
    public float minSpeed;
    public float minDistance;
    public float desiredDistance;
    public float distancePadding;

    protected bool hasCollided = false;

    void FixedUpdate(){
        this.hasCollided = false;
        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

        //if (distance <= this.minDistance) {
            //this.SetReward(RewardParameters.PUNISH_PlayerCollide);
            //this.resetFunction();
            //return;
        //}

        //if(distance < this.desiredDistance - this.distancePadding) {
        //if (distance <= this.minDistance) {
            //this.AddReward(RewardParameters.PUNISH_TooClose);
        //} else if(distance >= this.desiredDistance - this.distancePadding && distance <= this.desiredDistance + this.distancePadding) {
            //this.AddReward(RewardParameters.REWARD_Proximity);
        //}
        
    }

    void OnCollisionStay(Collision collision){
        if (this.hasCollided) {
            return;
        }

        this.hasCollided = true;
        string tag = collision.gameObject.tag;

        if(tag == "terrain"){
            this.AddReward(RewardParameters.PUNISH_TerrainCollide);
            //this.resetFunction();
            return;
        }
    }

    public override void OnActionReceived(float[] vectorAction) {
        base.OnActionReceived(vectorAction);
        
        int action0 = Mathf.FloorToInt(vectorAction[0]);
        int action1 = Mathf.FloorToInt(vectorAction[1]);

        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

        if (action0 == 0f && action1 == 0f) {
            
            if (distance >= this.desiredDistance - this.distancePadding && distance <= this.desiredDistance + this.distancePadding) {
                this.AddReward(RewardParameters.REWARD_Proximity);
            } 
        }

        if(this.shipBody.velocity.magnitude < this.minSpeed && distance > this.desiredDistance + this.distancePadding) {
            this.AddReward(RewardParameters.PUNISH_Inaction);
        }

        if(this.dizzyness > this.dizzyThreshold) {
            this.AddReward(RewardParameters.PUNISH_DIZZY);
        }

    }

    public void EndEpisodeReward() {
        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);
        
        if(distance <= this.minDistance) {
            this.SetReward(-1f);
        }

        if (distance < this.desiredDistance - this.distancePadding) {
            this.SetReward(0f);
            return;
        }

        if (distance <= this.desiredDistance + this.distancePadding) {
            this.SetReward(1f);
            return;
        }

        float distanceDifference = distance - (this.desiredDistance + distancePadding);

        if (distanceDifference > 500f) {
            this.SetReward(0f);
            return;
        }
        
        float reward = (1f - (distanceDifference / 500f)) * RewardParameters.REWARD_EndProximity;

        this.SetReward(reward);
    }

    public override void OnEpisodeBegin(){

    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;

        if (Input.GetKey(KeyCode.W)) {
            actionsOut[0] = 1f;
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            actionsOut[0] = 0f;
        }

        if (Input.GetKey(KeyCode.A)){
            actionsOut[1] = 2f;
        } else if(Input.GetKey(KeyCode.D)){
            actionsOut[1] = 1f;
        }
    }


}
