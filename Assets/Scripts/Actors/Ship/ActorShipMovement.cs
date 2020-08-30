using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class ActorShipMovement : AShipMovement
{
    [SerializeField] float maxTurn = 2f;
    [SerializeField] float brakeAmount = 0.98f;
    protected int canMove = 1;
    protected float acceleration = 0f;
    protected float turnAngle = 0f;
    protected float brake = 0f;

    protected Vector3 forwardBuffer;
    protected Vector3 horizontalScale = new Vector3(1, 0, 1);

    public void Init(GameObject waterPlane)
    {
        waterCalculator = waterPlane.GetComponent<WaterCalculator>();
    }

    public void ApplyControls(float acceleration, float turnAngle, float brake)
    {
        this.acceleration = acceleration;
        this.turnAngle = turnAngle;
        this.brake = brake;
    }

    public void SetCanMove(bool move){
        if(move){
            this.canMove = 1;
            return;
        }

        this.canMove = 0;
    }

    void FixedUpdate()
    {
        float steer = 0f;
        steer = 1 * this.turnAngle * this.canMove;

        if (this.Rigidbody.angularVelocity.y < this.maxTurn) {
            Rigidbody.AddTorque(steer * this.transform.up * this.steerPower);
        }

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

        if(this.brake > 0f) {
            this.Rigidbody.velocity = (
                Vector3.Scale(this.Rigidbody.velocity, this.horizontalScale) * this.brakeAmount
            ) + Vector3.Scale(this.Rigidbody.velocity, Vector3.up);

            this.Rigidbody.angularVelocity = (
                Vector3.Scale(this.Rigidbody.angularVelocity, Vector3.up) * this.brakeAmount
            ) + Vector3.Scale(this.Rigidbody.angularVelocity, this.horizontalScale);
        }
    }

    bool CanAccelerate(){
        if(waterCalculator == null || this.canMove == 0){
            return false;
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
