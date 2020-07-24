using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class ActorShipMovement : AShipMovement
{
    protected float acceleration = 0f;
    protected float turnAngle = 0f;

    void Start(){}

    void FixedUpdate()
    {
        Vector3 forceDirection = transform.forward;
        float steer = 0f;
        steer = 1 * this.turnAngle;

        Vector3 calc = steer * this.transform.up * this.steerPower;
        //Debug.Log($"turn: {this.turnAngle} , steer: {steer}, transform: {this.transform.up}, power: {this.steerPower}");
        //Debug.Log($"calc: {calc}");
        Rigidbody.AddTorque(calc);

        //Debug.Log(this.turnAngle);

        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        PhysicsHelper.ApplyForceToReachVelocity(
            Rigidbody, 
            forward * maxSpeed * this.acceleration, 
            power
        );
    }

    public void ControlShip(float acceleration, float turnAngle)
    {
        //Debug.Log($"calc: {calc}");

        this.acceleration = acceleration;
        this.turnAngle = turnAngle;
    }

}
