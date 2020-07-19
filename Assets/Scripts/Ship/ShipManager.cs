﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {
    
    public HunkManager hunkManager;
    public EquipmentManager equipmentManager;
    public PlayerShipMovement playerShipMovement;
    public BuoyancyManager buoyancyManager;

    protected InputMaster controls;

    void Start()
    {
        if (InputRunner.Instance != null)
        {
            // Search for singleton instance of InputRunner
            controls = InputRunner.Instance.controls;
        }
        else
        {
            // If it doesn't exist THEN make a new one. This will make it to where you can test the player and movement in any scene without all the back end architecture.
            InputRunner runner = new InputRunner();
            if (runner != null)
                controls = runner.controls;
        }
    }

    void Update()
    {        
    }

    public void Init(InputMaster controls, GameObject waterPlane)
    {
        this.controls = controls;
        equipmentManager.Init(controls);
        playerShipMovement.Init(controls);
        buoyancyManager.Init(waterPlane);
        hunkManager.Init(null);
    }
}
