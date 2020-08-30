using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunkTrain : MonoBehaviour {
    public ShipAgentTrain agent;

    protected float hitReward;

    public void TrainInit(ShipAgentTrain agent, int hunkCount){
        this.agent = agent;
    }

    public void Reset(){
        this.gameObject.SetActive(true);
    }

    void OnCollisionEnter(Collision collision){
        if (!this.gameObject.activeSelf) {
            return;
        }

        if (collision.gameObject.tag != "projectile"){
            return;
        }

        CannonBall projectile = collision.gameObject.GetComponent<CannonBall>();
        ShipAgentTrain owner = projectile.owner.GetComponent<ShipAgentTrain>();
        owner.AddReward(RewardParameters.REWARD_HitHunk);

        this.gameObject.SetActive(false);
    }
}
