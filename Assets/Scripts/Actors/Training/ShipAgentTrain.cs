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
    public float rewardDistance = 500f;
    public float rewardAngle = 60f;

    public override void OnActionReceived(float[] vectorAction) {
        base.OnActionReceived(vectorAction);

        float distance = Vector3.Distance(this.transform.position, this.playerObject.transform.position);
        float distanceZone = this.DiscreteDistance(distance);
        float desiredZone = this.DiscreteDistance(this.desiredDistance);
        float minZone = this.DiscreteDistance(this.minDistance);
        
        if(distanceZone <= minZone) {
            this.AddReward(RewardParameters.PUNISH_TooClose);
        } else if (distanceZone <= desiredZone) {

            int action0 = Mathf.FloorToInt(vectorAction[0]);
            int action1 = Mathf.FloorToInt(vectorAction[1]);

            float angle = this.PerceiveAngle(
                Vector3.forward,
                this.transform.InverseTransformPoint(this.playerObject.transform.position).normalized,
                Vector3.up
            );

            float angleZone = this.DiscretizeAngle(this.rewardAngle);

            if (action0 == 1 && action1 == 0) {
                this.AddReward(RewardParameters.REWARD_Proximity);
            }
        } else {
            this.AddReward(RewardParameters.PUNISH_Inaction);
        }
    }

    public void EndEpisodeReward() {
        float distance = this.transform.InverseTransformPoint(this.playerObject.transform.position).magnitude;

        float distanceZone = this.DiscreteDistance(distance);
        float desiredZone = this.DiscreteDistance(this.desiredDistance);
        float rewardZone = this.DiscreteDistance(this.rewardDistance);
        float minZone = this.DiscreteDistance(this.minDistance);

        if (distanceZone <= minZone) {
            this.SetReward(RewardParameters.PUNISH_EndTooClose);
            return;
        }

        if (distanceZone <= desiredZone) {
            this.SetReward(RewardParameters.REWARD_EndProximity);
            return;
        }

        float zoneDifference = distanceZone - desiredZone;

        if (zoneDifference > rewardZone) {
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

    public override void Init(
        Brain brain,
        GameObject playerObject
    ) {
        base.Init(brain, playerObject);

        TargetShip target = playerObject.GetComponent<TargetShip>();

        float minZoneDistance = this.DiscreteDistance(this.minDistance) * this.operationalDistance * 2;
        float desiredZoneDistance = this.DiscreteDistance(this.desiredDistance) * this.operationalDistance * 2;
        float rewardZoneDistance = this.DiscreteDistance(this.desiredDistance + this.rewardDistance) * this.operationalDistance * 2;

        target.UpdateZones(minZoneDistance, desiredZoneDistance, rewardZoneDistance);
    }
}
