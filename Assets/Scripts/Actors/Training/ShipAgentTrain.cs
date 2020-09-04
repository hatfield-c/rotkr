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
    public float aimPadding;

    protected bool hasCollided = false;

    void FixedUpdate(){
        this.hasCollided = false;
        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);

        if (distance <= this.minDistance) {
            this.SetReward(RewardParameters.PUNISH_MinDistance);
            this.resetFunction();
            return;
        }
        
       if(distance < this.desiredDistance - this.distancePadding) {
            this.AddReward(
                RewardParameters.PUNISH_Frame * 
                (1 - (
                        Mathf.Clamp((this.desiredDistance - distancePadding - distance - this.minDistance), 0, 1) / (this.desiredDistance - this.distancePadding)
                    )
                )
            );
        } else if (distance > this.desiredDistance + this.distancePadding) {
            this.AddReward(
                RewardParameters.PUNISH_Frame *
                (
                    (distance - this.desiredDistance - distancePadding) / (distance)
                )
            );
        } else if(distance >= this.desiredDistance - this.distancePadding && distance <= this.desiredDistance + this.distancePadding) {
            this.AddReward(RewardParameters.REWARD_Proximity);

            this.vectorBuffer = this.transform.InverseTransformPoint(this.playerObject.transform.position);
            float dotResult = Vector3.Dot(Vector3.forward.normalized, this.vectorBuffer.normalized);

            if(dotResult >= -this.aimPadding && dotResult <= this.aimPadding) {
                this.AddReward(RewardParameters.REWARD_Aimed);
            }
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

    public override void OnEpisodeBegin(){

    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[2] = 0f;
        actionsOut[3] = 0f;

        if (Input.GetKey(KeyCode.W)){
            actionsOut[0] = 1f;
        } else if(Input.GetKey(KeyCode.S)){
            actionsOut[0] = 2f;
        }

        if(Input.GetKey(KeyCode.A)){
            actionsOut[1] = 2f;
        } else if(Input.GetKey(KeyCode.D)){
            actionsOut[1] = 1f;
        }

        if(Input.GetKey(KeyCode.Space)){
            actionsOut[2] = 1f;   
        }

        if (Input.GetKey(KeyCode.LeftControl)) {
            actionsOut[3] = 1f;
        }
    }


}
