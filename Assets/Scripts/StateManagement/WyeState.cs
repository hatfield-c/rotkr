using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum TypeOfWye { None, CollectionChamber, Spillway };
public class WyeState : AGameState
{
    public WyeState(WyeData data)
    {
        Data = data;
    }

    #region references
    public WyeData Data;
    #endregion

    #region handlers
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
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
                SceneManager.LoadScene(Data.WyeType.ToString());
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