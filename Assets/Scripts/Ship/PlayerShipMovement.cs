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
        Vector3 forceDirection = transform.forward;
        float steer = 0f;
        steer = 1 * shipDirection.x;

        Rigidbody.AddTorque(steer * this.transform.up * this.steerPower);

        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        PhysicsHelper.ApplyForceToReachVelocity(Rigidbody, forward * maxSpeed * shipDirection.y, power);
    }
    #endregion

    #region public functions
    public void Init(InputMaster controls)
    {
        registerInput(controls);
    }
    #endregion

    #region private functions
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
