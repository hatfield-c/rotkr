using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatDeadzone {
    public float floatMax = 1f;
    public float floatMin = -1f;
    public float deadMax = 0.25f;
    public float deadMin = -0.25f;

    protected Transform waterLevel;

    public void Init(Transform waterLevel){
        this.waterLevel = waterLevel;
    }

    public float stableForce(float velocityY, float yPos){

        if(!this.isInFloatZone(yPos)){
            return 0;
        }

        float dir = 0;
        if(velocityY < 0){
            dir = 1;
        } else if(velocityY > 0){
            dir = -1;
        }

        float force = -Physics.gravity.y;
        return force * dir * this.distanceMultiplier(yPos);
    }

    public float distanceMultiplier(float yPos){
        float relativePos = yPos - this.getWaterLevel();

        if(this.isUpperFloat(yPos)){
            float scaledPos = (relativePos - this.deadMax) / Mathf.Abs(this.floatMax - this.deadMax);

            return scaledPos;
        }

        if(this.isLowerFloat(yPos)){
            float scaledPos = (this.deadMin - relativePos) / Mathf.Abs(this.deadMin - this.floatMin);

            return scaledPos;
        }

        return 0;
    }

    public bool isUnderWater(float yPos){
        if(yPos < this.floatMin + this.getWaterLevel()){
            return true;
        }

        return false;
    }

    public bool isAboveWater(float yPos){
        if(yPos > this.floatMax + this.getWaterLevel()){
            return true;
        }

        return false;
    }

    public bool isAboveDead(float yPos){
        if(yPos > this.deadMax + this.getWaterLevel()){
            return true;
        }

        return false;
    }

    public bool isUnderDead(float yPos){
        if(yPos < this.deadMin + this.getWaterLevel()){
            return true;
        }

        return false;
    }

    public bool isInFloatZone(float yPos){
        if(yPos < this.floatMax + this.getWaterLevel() && yPos > this.floatMin + this.getWaterLevel()){
            return true;
        }

        return false;
    }

    public bool isInDeadZone(float yPos){
        if(yPos < this.deadMax + this.getWaterLevel() && yPos > this.deadMin + this.getWaterLevel()){
            return true;
        }

        return false;
    }

    public bool isUpperFloat(float yPos){
        if(this.isAboveDead(yPos) && this.isInFloatZone(yPos)){
            return true;
        }

        return false;
    }

    public bool isLowerFloat(float yPos){
        if(this.isUnderDead(yPos) && this.isInFloatZone(yPos)){
            return true;
        }

        return false;
    }

    public float getWaterLevel(){
        return this.waterLevel.position.y;
    }
}
