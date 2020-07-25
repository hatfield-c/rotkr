using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {
    
    public HunkManager HunkManager;
    public EquipmentManager EquipmentManager;
    public PlayerShipMovement PlayerShipMovement;
    public BuoyancyManager BuoyancyManager;

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
        EquipmentManager.Init(controls);
        PlayerShipMovement.Init(controls);
        BuoyancyManager.Init(waterPlane);
        HunkManager.Init(null);
    }
}
