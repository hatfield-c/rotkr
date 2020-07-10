using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfWye { None, CollectionChamber, Spillway };
public class WyeState : AGameState
{
    public WyeData Data;
    public WyeState(WyeData data)
    {
        Data = data;
    }

    #region references
    #endregion

    #region handlers
    #endregion

    #region logic

    #endregion

    public override void Execute()
    {
        Debug.Log($"Execute called for {Data.WyeType} wye.");
        //ExecuteComplete?.Invoke();
    }
    public override void Cancel()
    {
        CancelComplete?.Invoke();
    }
}

public class WyeData
{
    public TypeOfWye WyeType;
}