using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class InputRunner
{
    public InputRunner()
    {
        controls = new InputMaster();
        controls.Player.Movement.performed += context => OnPlayerMovement(context.ReadValue<Vector2>());
        controls.Enable();
    }
    ~InputRunner()
    {
        controls.Player.Movement.performed -= context => OnPlayerMovement(context.ReadValue<Vector2>());
        controls.Disable();
    }

    #region references
    InputMaster controls;
    PlayerShipMovement playerShipMovement;
    #endregion

    #region handlers
    void OnPlayerMovement(Vector2 inputDirection)
    {
        if(playerShipMovement != null)
            playerShipMovement.UpdateShipDirection(inputDirection);
    }
    #endregion

    #region public functions
    public void UpdateReferences(PlayerShipMovement playerShip)
    {
        playerShipMovement = playerShip;
    }
    #endregion
}
