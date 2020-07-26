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
        UnsubscribeAll();
    }
    #region variables
    bool success;
    #endregion


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
            gate.PlayerEnteredTheEnd += () =>
            {
                success = true;
                UnsubscribeAll();
                ExecuteComplete?.Invoke();
            };
        }

        // Subscribe to when the Wye is completely sunk (underwater)
        refs.WyeSinker.WyeCompletelySunk += () =>
        {
            UnsubscribeAll();
            ExecuteComplete?.Invoke();
        };
        //ExecuteComplete?.Invoke();
    }
    public override void Cancel()
    {
        CancelComplete?.Invoke();
    }

    public bool GetSuccess()
    {
        return success;
    }
    #endregion

    #region private functions
    void UnsubscribeAll()
    {
        foreach(EndGate gate in refs.EndGates)
            gate.PlayerEnteredTheEnd = null;
        refs.WyeSinker.WyeCompletelySunk = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion
}


/// <summary>
/// Used to store and load data in <see cref="WyeState"/>.
/// </summary>
public class WyeData
{
    /// <summary>
    /// Creates a new <see cref="WyeData"/> of type <paramref name="type"/>.
    /// </summary>
    /// <param name="type"></param>
    public WyeData(TypeOfWye type)
    {
        AssignID();
        WyeType = type;
    }

    /// <summary>
    /// Creates a new <see cref="WyeData"/> with random attributes if <paramref name="createRandom"/> is true.
    /// </summary>
    /// <param name="createRandom"></param>
    public WyeData(bool createRandom)
    {
        AssignID();
        if (createRandom)
        {
            WyeType = TypeOfWye.CollectionChamber;
            if (UnityEngine.Random.Range(0, 1f) > .5f)
                WyeType = TypeOfWye.Spillway;
        }
    }

    void AssignID()
    {
        if(IDs == null)
        {
            IDs = new List<int>();
            Debug.Log("<color=cyan>Creating new ID list</color>");
        }
            
        ID = IDs.Count;
        IDs.Add(ID);
        Debug.Log($"<color=cyan>Assigning ID: {ID}. IDs.Count is now: {IDs.Count}</color>");
    }
    public TypeOfWye WyeType;
    public int ID;
    public static List<int> IDs;
}