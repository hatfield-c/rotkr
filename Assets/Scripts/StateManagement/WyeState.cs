﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum TypeOfWye { None, CollectionChamber, Spillway };
public class WyeState : AGameState
{
    public WyeState(WyeData data, GameObject playerPrefab, InputMaster controls)
    {
        Data = data;
        this.playerPrefab = playerPrefab;
        this.controls = controls;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    ~WyeState()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region references
    public WyeData Data;
    WyeStateReferences refs;
    InputMaster controls;

    GameObject playerPrefab;
    GameObject player;
    ShipManager ship;
    #endregion

    #region handlers
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update references
        refs = GameObject.FindObjectOfType<WyeStateReferences>();
    }
    #endregion

    #region logic

    #endregion

    #region public functions
    public override void Execute()
    {
        Debug.Log($"Execute called for {Data.WyeType} wye.");
        // Spawn the player
        if (playerPrefab != null)
        {
            // Choose a random spawn point to spawn the player
            int spawnIndex = UnityEngine.Random.Range(0, refs.SpawnPoints.Count);

            player = GameObject.Instantiate(playerPrefab, refs.SpawnPoints[spawnIndex].position, refs.SpawnPoints[spawnIndex].rotation) as GameObject;
            ship = player.GetComponent<ShipManager>();
            ship.Init(controls, refs.WaterPlane);
        }
        else
            Debug.LogError($"Tried to spawn the player but there is no prefab");

        // Subscribe to End Gate events
        foreach(EndGate gate in refs.EndGates)
        {
            gate.PlayerEnteredTheEnd += () => { ExecuteComplete?.Invoke(); };
        }
        //ExecuteComplete?.Invoke();
    }
    public override void Cancel()
    {
        CancelComplete?.Invoke();
    }
    #endregion
}

public class WyeData
{
    public TypeOfWye WyeType;
}