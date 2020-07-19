using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatDeadzone {
    public float floatMax = 1f;
    public float floatMin = -1f;
    public float deadMax = 0.25f;
    public float deadMin = -0.25f;

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

        float force = -Physics.gravity.y;
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
}
