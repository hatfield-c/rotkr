using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRat : MonoBehaviour
{
    public ShipAgentTrain agent;

    protected float hitReward;

    public void TrainInit(ShipAgentTrain agent, int ratCount){
        this.agent = agent;

        this.hitReward = RewardParameters.REWARD_HitRat;
    }
    
    public void Reset(){
        this.gameObject.SetActive(true);
    }

    void OnCollisionEnter(Collision collision){
        if (!this.gameObject.activeSelf) {
            return;
        }

        if(collision.gameObject.tag != "projectile"){
            return;
        }

        CannonBall projectile = collision.gameObject.GetComponent<CannonBall>();
        ShipAgentTrain owner = projectile.owner.GetComponent<ShipAgentTrain>();
        owner.AddReward(this.hitReward);

        this.gameObject.SetActive(false);

        owner.resetFunction();
    }
}
