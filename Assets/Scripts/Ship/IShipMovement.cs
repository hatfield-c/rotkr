﻿using UnityEngine;

public interface IShipMovement
{
    // TODO: Add MoveTowards(Vector3)
    // TODO: Add TurnTowardsPoint(Vector3)
}

public abstract class AShipMovement: MonoBehaviour, IShipMovement
{
    [HeaderAttribute("Ship Movement Parameters")]
    /// <summary>
    /// Location of the motor driving the ship
    /// </summary>
    [SerializeField] protected Transform motor;

    /// <summary>
    /// Affects how fast the ship can turn
    /// </summary>
    [SerializeField] protected float steerPower;

    /// <summary>
    /// Affects how fast the ship accelerates
    /// </summary>
    [SerializeField] protected float power;

    /// <summary>
    /// Fastest speed the motor can push the ship
    /// </summary>
    [SerializeField] protected float maxSpeed;

    /// <summary>
    /// Loss in velocity due to drag from air and water.
    /// </summary>
    [SerializeField] protected float drag;

    /// <summary>
    /// Reference to this ship's <see cref="Rigidbody"/>
    /// </summary>
    [SerializeField] protected Rigidbody Rigidbody;

    /// <summary>
    /// Starting Rotation of the Motor
    /// </summary>
    protected Quaternion startRotation;
    protected void Awake()
    {
        if(motor != null)
        {
            startRotation = motor.localRotation;
        }
        else
        {
            Debug.LogError($"Motor variable on {this.gameObject.name} has not been set. Start Rotation not initialized");
        }
    }
}
