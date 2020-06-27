using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuoyancyParameters
{
    public float force = 9.8f;
    public float forceRatio = 1f;
    public float forceBias = 1f;
    public float threshold = 0f;

    public float calculate(float depth){
        return (forceRatio / (Mathf.Exp( (forceRatio - (depth - threshold)) * forceBias))) * this.force;
    }
}
