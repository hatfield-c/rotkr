using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunkTrain : MonoBehaviour {
    public ShipAgentTrain agent;

    protected float breakForce;

    public void TrainInit(ShipAgentTrain agent, float breakForce){
        this.agent = agent;
        this.breakForce = breakForce;
    }

    public void Reset(){
        this.gameObject.SetActive(true);
    }

    void OnCollision(Collision collision){
        float force = collision.impulse.magnitude / Time.fixedDeltaTime;

        if(force >= this.breakForce){
            this.gameObject.SetActive(false);

            //todo: add reward
        }
    }
}
