using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRat : MonoBehaviour
{
    public ShipAgentTrain agent;

    protected float breakForce;
    protected float hitReward;
    protected float breakReward;

    public void TrainInit(ShipAgentTrain agent, float breakForce, int ratCount){
        this.agent = agent;
        this.breakForce = breakForce;

        this.hitReward = RewardParameters.REWARD_HitRat / ratCount;
        this.breakReward = RewardParameters.REWARD_BreakRat / ratCount;
    }
    
    public void Reset(){
        this.gameObject.SetActive(true);
    }

    void OnCollisionEnter(Collision collision){
        float force = collision.impulse.magnitude / Time.fixedDeltaTime;

        if(collision.gameObject.tag != "projectile"){
            return;
        }

        CannonBall projectile = collision.gameObject.GetComponent<CannonBall>();
        ShipAgentTrain owner = projectile.owner.GetComponent<ShipAgentTrain>();
        owner.AddReward(this.hitReward);

        if(force >= this.breakForce){
            this.gameObject.SetActive(false);

            owner.AddReward(this.breakReward);
            owner.resetFunction();
        }
    }
}
