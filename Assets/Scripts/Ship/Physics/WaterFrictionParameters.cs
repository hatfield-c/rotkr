using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaterFrictionParameters
{
    public float maxForce = 10f;
    public float rate = 1f;

    public float frictionForce(float velocity){
        float force = rate * Mathf.Pow(velocity, 2);
 
        if(force > this.maxForce){
            return this.maxForce;
        }
        
        return force;
    }
}
