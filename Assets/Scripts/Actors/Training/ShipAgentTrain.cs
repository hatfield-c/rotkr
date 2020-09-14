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
    public float rewardDistance = 500f;

    public override void OnActionReceived(float[] vectorAction) {
        base.OnActionReceived(vectorAction);

        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);
        float distanceZone = this.DiscreteDistance(distance);
        float desiredZone = this.DiscreteDistance(this.desiredDistance + this.distancePadding);
        float minZone = this.DiscreteDistance(this.minDistance);
        
        if(distanceZone <= minZone) {
            this.AddReward(RewardParameters.PUNISH_TooClose);
        } else if (distanceZone <= desiredZone) {
            this.AddReward(RewardParameters.REWARD_Proximity);
        } else {
            this.AddReward(RewardParameters.PUNISH_Inaction);
        }
    }

    public void EndEpisodeReward() {
        //float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);
        float distance = this.transform.InverseTransformPoint(this.playerObject.transform.position).magnitude;

        float distanceZone = this.DiscreteDistance(distance);
        float desiredZone = this.DiscreteDistance(this.desiredDistance + this.distancePadding);
        float rewardZone = this.DiscreteDistance(this.rewardDistance);

        //if (distance <= this.minDistance) {
            //this.SetReward(-0.1f);
            //return;
        //}

        if (distanceZone <= desiredZone) {
            this.SetReward(1f);
            return;
        }

        //float distanceDifference = distance - (this.desiredDistance + this.distancePadding);
        float zoneDifference = distanceZone - desiredZone;

        if (zoneDifference > rewardZone) {

            //this.SetReward(-1f);
            return;
        } 
        
        float reward = ((rewardZone - zoneDifference) / rewardZone) * RewardParameters.REWARD_EndProximity;

        this.SetReward(reward);
    }

    public override void CollectObservations(VectorSensor sensor) {
        base.CollectObservations(sensor);

        foreach (ASensor raySensor in this.sensors) {
            float zone = raySensor.ReadSensorPool();

            if(zone == raySensor.GetMinZone()) {
                this.AddReward(RewardParameters.PUNISH_TerrainCollide);
            }
        }
    }
}
