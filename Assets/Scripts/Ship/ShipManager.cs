﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    TextMeshProUGUI scrapDisplay;

    void Start()
    {
        if (Debug)
        {
            InputRunner runner = new InputRunner();
            Init(new ShipData(), runner.controls, FindObjectOfType<WaterCalculator>().gameObject, null, null, null);
        }
    }

    void FixedUpdate() { }

    public void Init(ShipData data, InputMaster controls, GameObject waterPlane, RectTransform ratHealthGroup, TextMeshProUGUI scrapDisplay, Action allDeadCallback)
    {
        this.data = data;
        this.controls = controls;
        this.scrapDisplay = scrapDisplay;

        this.data.ScrapData.ScrapUpdated = UpdateScrapDisplay;
        this.data.UpgradeData.GlueBreakForceUpdated = UpdateHardenedGlue;
        this.data.UpgradeData.ReloadSpeedRatioUpdated = UpdateReloadSpeed;

        UpdateHardenedGlue(this.data.UpgradeData.GetNewBreakForce());
        UpdateReloadSpeed(this.data.UpgradeData.GetNewReloadRatio());

        equipmentManager.Init(controls);
        playerShipMovement.Init(controls, waterPlane);
        buoyancyManager.Init(waterPlane);
        hunkManager.Init(data.HunkDatum);
        ratGroupManager.Init(data, shipReferences, waterPlane, ratHealthGroup, allDeadCallback);

        UpdateScrapDisplay();

        this.controls.Player.Quit.performed += context => AGameState.EndGame();
    }
    public ShipData GetData()
    {
        return data;
    }

    public ShipReferences GetShipReferences()
    {
        return shipReferences;
    }

    public List<Hunk> GetDeletedHunks(){
        return hunkManager.GetDeletedHunks();
    }

    public void AddScrap(int value)
    {
        data.ScrapData.AddScrap(value);
        if (scrapDisplay == null) return;
        scrapDisplay.text = data.ScrapData.GetScrap().ToString();
    }
    void UpdateHardenedGlue(float newBreakForce)
    {
        hunkManager.UpdateHunkBreakForce(newBreakForce);
    }
    void UpdateReloadSpeed(float newReloadRatio)
    {
        equipmentManager.UpdateCannonReloads(newReloadRatio);
    }
    void UpdateScrapDisplay()
    {
        if (scrapDisplay == null) return;
            scrapDisplay.text = data.ScrapData.GetScrap().ToString();
    }
}