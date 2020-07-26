using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {
    
    [SerializeField] HunkManager hunkManager = null;
    [SerializeField] EquipmentManager equipmentManager = null;
    [SerializeField] PlayerShipMovement playerShipMovement = null;
    [SerializeField] BuoyancyManager buoyancyManager = null;

    /// <summary>
    /// Check this true if using the player in a non-game State
    /// </summary>
    public bool Debug;
    protected InputMaster controls;

    void Start()
    {
        if (Debug)
        {
            InputRunner runner = new InputRunner();
            Init(runner.controls, FindObjectOfType<WaterCalculator>().gameObject);
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
