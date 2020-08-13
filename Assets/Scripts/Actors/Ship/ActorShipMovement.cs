using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class ActorShipMovement : AShipMovement
{
    protected float acceleration = 0f;
    protected float turnAngle = 0f;

    protected Vector3 forwardBuffer;
    protected Vector3 horizontalScale = new Vector3(1, 0, 1);

    public void Init(GameObject waterPlane)
    {
        waterCalculator = waterPlane.GetComponent<WaterCalculator>();
    }

    public void ApplyControls(float acceleration, float turnAngle)
    {
        this.acceleration = acceleration;
        this.turnAngle = turnAngle;
    }

    void FixedUpdate()
    {
        float steer = 0f;
        steer = 1 * this.turnAngle;

        Rigidbody.AddTorque(steer * this.transform.up * this.steerPower);

        if(CanAccelerate()){
            forwardBuffer = Vector3.Scale(horizontalScale, transform.forward);
        } else {
            forwardBuffer = Vector3.zero;
        }

        PhysicsHelper.ApplyForceToReachVelocity(
            Rigidbody, 
            forwardBuffer * maxSpeed * this.acceleration, 
            power
        );
    }

    bool CanAccelerate(){
        if(waterCalculator == null){
            return true;
        }

        float height = waterCalculator.calculateHeight(
            motor.position.x,
            motor.position.z
        );

        if(motor.position.y > height + cutoffThreshold){
            return false;
        }

        return true;
    }

}
