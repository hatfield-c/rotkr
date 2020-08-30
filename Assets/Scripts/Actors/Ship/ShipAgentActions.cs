using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAgentActions
{
    protected float acceleration;
    protected float turnDirection;
    protected float shoot;
    protected float brake;

    public ShipAgentActions(float acceleration, float turnDirection, float shoot, float brake){
        this.acceleration = acceleration;
        this.turnDirection = turnDirection;
        this.shoot = shoot;
        this.brake = brake;
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

    public float GetBrake() {
        return this.brake;
    }
}
