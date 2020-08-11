using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
/// <summary>
/// Defines how the PLAYER ship will move
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class PlayerShipMovement : AShipMovement
{
    Vector2 shipDirection = Vector2.zero;

    #region references
    InputMaster controls;
    #endregion

    #region blackboard
    Vector3 forwardBuffer;
    #endregion

    #region constants
    Vector3 horizontalScale = new Vector3(1, 0, 1);
    #endregion

    #region handlers
    void OnEnable()
    {
        if(controls != null)
            controls.Player.Movement.performed += context => OnPlayerMovement(context.ReadValue<Vector2>());
    }
    void OnDisable()
    {
        if(controls != null)
            controls.Player.Movement.performed -= context => OnPlayerMovement(context.ReadValue<Vector2>());
    }
    #endregion

    #region logic
    void Start(){}

    void FixedUpdate()
    {
        float steer = 0f;
        steer = 1 * shipDirection.x;

        Rigidbody.AddTorque(steer * this.transform.up * this.steerPower);


        if(CanAccelerate()){
            forwardBuffer = Vector3.Scale(horizontalScale, transform.forward);
        } else {
            forwardBuffer = Vector3.zero;
        }

        PhysicsHelper.ApplyForceToReachVelocity(Rigidbody, forwardBuffer * maxSpeed * shipDirection.y, power);
    }
    #endregion

    #region public functions
    public void Init(InputMaster controls, GameObject waterPlane)
    {
        registerInput(controls);
        waterCalculator = waterPlane.GetComponent<WaterCalculator>();
    }
    #endregion

    #region private functions
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

    void registerInput(InputMaster controls)
    {
        this.controls = controls;
        this.controls.Player.Movement.performed += context => OnPlayerMovement(context.ReadValue<Vector2>());
    }
    void OnPlayerMovement(Vector2 inputDirection)
    {
        shipDirection = inputDirection;
    }
    #endregion
}
