using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAgentActions
{
    protected float acceleration;
    protected float turnDirection;
    protected float shoot;

    public ShipAgentActions(float acceleration, float turnDirection, float shoot){
        this.acceleration = acceleration;
        this.turnDirection = turnDirection;
        this.shoot = shoot;
    }

    public float GetAcceleration(){
        return this.acceleration;
    }

    public float GetTurnDirection(){
        return this.turnDirection;
    }

    public float GetShoot(){
        return this.shoot;
    }
}
