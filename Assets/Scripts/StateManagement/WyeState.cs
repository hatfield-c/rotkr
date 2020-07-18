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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    ~WyeState()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region references
    public WyeData Data;
    #endregion

    #region handlers
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update references
    }
    #endregion

    #region logic

    #endregion

    #region public functions
    public override void Execute()
    {
        Debug.Log($"Execute called for {Data.WyeType} wye.");
        if (Data == null)
            return;
        // Load the actual scene you are
        switch (Data.WyeType)
        {
            case TypeOfWye.None:
                break;
            default:
                //SceneManager.LoadScene(Data.WyeType.ToString());
                break;
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