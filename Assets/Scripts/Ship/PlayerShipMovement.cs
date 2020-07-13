using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Defines how the PLAYER ship will move
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class PlayerShipMovement : AShipMovement
{

    InputMaster controls;
    Vector2 shipDirection = Vector2.zero;
    new void Awake()
    {
        base.Awake();
        controls = new InputMaster();
        controls.Player.Movement.performed += context => Move(context.ReadValue<Vector2>());
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        
    }
    void Move(Vector2 inputDirection)
    {
        shipDirection = inputDirection;
    }

    void FixedUpdate()
    {
        Vector3 forceDirection = transform.forward;
        float steer = 0f;
        steer = 1 * shipDirection.x;

        Rigidbody.AddTorque(steer * this.transform.up * this.steerPower);

        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);
        PhysicsHelper.ApplyForceToReachVelocity(Rigidbody, forward * maxSpeed * shipDirection.y, power);
    }
}
