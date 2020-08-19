using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class FloatDeadzone {
    public float floatMax = 1f;
    public float floatMin = -1f;
    public float deadMax = 0.25f;
    public float deadMin = -0.25f;
    public float stabilizingForce = Physics.gravity.y;
    
    protected FloatZoneParameters origParameters = new FloatZoneParameters(); 

    public FloatDeadzone(){
        this.origParameters.SetParameters(
            this.floatMax,
            this.floatMin,
            this.deadMax,
            this.deadMin,
            this.stabilizingForce
        );
    }

    public float stableForce(float velocityY, float yPos, float target){

        if(!this.isInFloatZone(yPos, target)){
            return 0;
        }

        float dir = 0;
        if(velocityY < 0){
            dir = 1;
        } else if(velocityY > 0){
            dir = -1;
        }

        float force = -this.stabilizingForce;
        return force * dir * this.distanceMultiplier(yPos, target);
    }

    public float distanceMultiplier(float yPos, float target){
        float relativePos = yPos - target;

        if(this.isUpperFloat(yPos, target)){
            float scaledPos = (relativePos - this.deadMax) / Mathf.Abs(this.floatMax - this.deadMax);

            return scaledPos;
        }

        if(this.isLowerFloat(yPos, target)){
            float scaledPos = (this.deadMin - relativePos) / Mathf.Abs(this.deadMin - this.floatMin);

            return scaledPos;
        }

        return 0;
    }

    public bool isUnderWater(float yPos, float target){
        if(yPos < this.floatMin + target){
            return true;
        }

        return false;
    }

    public bool isAboveWater(float yPos, float target){
        if(yPos > this.floatMax + target){
            return true;
        }

        return false;
    }

    public bool isAboveDead(float yPos, float target){
        if(yPos > this.deadMax + target){
            return true;
        }

        return false;
    }

    public bool isUnderDead(float yPos, float target){
        if(yPos < this.deadMin + target){
            return true;
        }

        return false;
    }

    public bool isInFloatZone(float yPos, float target){
        if(yPos < this.floatMax + target && yPos > this.floatMin + target){
            return true;
        }

        return false;
    }

    public bool isInDeadZone(float yPos, float target){
        if(yPos < this.deadMax + target && yPos > this.deadMin + target){
            return true;
        }

        return false;
    }

    public bool isUpperFloat(float yPos, float target){
        if(this.isAboveDead(yPos, target) && this.isInFloatZone(yPos, target)){
            return true;
        }

        return false;
    }

    public bool isLowerFloat(float yPos, float target){
        if(this.isUnderDead(yPos, target) && this.isInFloatZone(yPos, target)){
            return true;
        }

        return false;
    }

    public void Sink(float sinkAmount, float sinkForce){
        this.floatMin -= sinkAmount;
        this.deadMin -= sinkAmount;
        this.deadMax -= sinkAmount;
        this.stabilizingForce = sinkForce;
    }

    public void Reset(){
        this.floatMax = this.origParameters.floatMax;
        this.floatMin = this.origParameters.floatMin;
        this.deadMax = this.origParameters.deadMax;
        this.deadMin = this.origParameters.deadMin;
        this.stabilizingForce = this.origParameters.stabilizingForce;
    }

}
