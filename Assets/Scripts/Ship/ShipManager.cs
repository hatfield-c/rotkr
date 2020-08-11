using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour {
    
    [SerializeField] HunkManager hunkManager = null;
    [SerializeField] EquipmentManager equipmentManager = null;
    [SerializeField] ShipReferences shipReferences = null;
    [SerializeField] PlayerShipMovement playerShipMovement = null;
    [SerializeField] BuoyancyManager buoyancyManager = null;
    [SerializeField] RatGroupManager ratGroupManager = null;

    ShipData data;

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
            Init(new ShipData(), runner.controls, FindObjectOfType<WaterCalculator>().gameObject);
        }
    }

    void Update() { }

    public void Init(ShipData data, InputMaster controls, GameObject waterPlane)
    {
        this.data = data;
        this.controls = controls;
        equipmentManager.Init(controls);
        playerShipMovement.Init(controls, waterPlane);
        buoyancyManager.Init(waterPlane);
        hunkManager.Init(data.HunkDatum);
        ratGroupManager.Init(data, shipReferences, waterPlane);
    }
    public ShipData GetData()
    {
        return data;
    }

    public ShipReferences GetShipReferences()
    {
        return shipReferences;
    }
}