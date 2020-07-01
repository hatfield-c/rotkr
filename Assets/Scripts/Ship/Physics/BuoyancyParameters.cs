using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuoyancyParameters
{
    public float force = 9.8f;
    public float damping = 0.0f;
    public float maxDepth = 1.0f;

    public float buoyantForce(float depth){
        return this.force * Mathf.Exp(depth * this.damping);
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
}
