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

    [HeaderAttribute("Player Input Keys")]
    public KeyCode ForwardKey = KeyCode.W;
    public KeyCode BackKey = KeyCode.S;
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode RightKey = KeyCode.D;

    void Start()
    {
        
    }

    // TODO: Change the Movement Input to read from axis instead of keys
    void FixedUpdate()
    {
        Vector3 forceDirection = transform.forward;
        float steer = 0f;
        if (Input.GetKey(LeftKey))
            steer = 1;
        if (Input.GetKey(RightKey))
            steer = -1;

        Rigidbody.AddForceAtPosition(steer * transform.right * steerPower, motor.position);

        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);

        if (Input.GetKey(ForwardKey))
            PhysicsHelper.ApplyForceToReachVelocity(Rigidbody, forward * maxSpeed, power);
        if (Input.GetKey(BackKey))
            PhysicsHelper.ApplyForceToReachVelocity(Rigidbody, forward * -maxSpeed, power);

    }
}
