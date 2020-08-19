using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuoyancyParameters
{
    public float force = 9.8f;
    public float forceRatio = 1f;
    public float forceBias = 0.0f;
    public float threshold = 0.0f;

    public float torqueDamping = 1.0f;
    public float maxDepth = 1.0f;
    public float maxForce = 10000f;
    public float maxVelocity = 20f;

    protected float origForce;

    public BuoyancyParameters(){
        this.origForce = this.force;
    }

    public float buoyantForce(float depth){
        return this.force * (forceRatio / (Mathf.Exp( (forceRatio - (depth - threshold)) * forceBias)));
    }

    public float hammerForce(float stopDistance, float velocity, float mass){
        float acceleration = Mathf.Pow(velocity, 2) / (2 * stopDistance);
        return mass * acceleration;
    }

    public float stopDistance(float depth, float velocity, float mass){
        float acceleration = this.force / mass;
        float timeToStop = velocity / acceleration;
        return (velocity * timeToStop) - (0.5f * acceleration * Mathf.Pow(timeToStop, 2));
    }

    public float GetOrigForce(){
        return this.origForce;
    }
}
