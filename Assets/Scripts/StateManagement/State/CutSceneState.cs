using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State is responsible for managing the video that is playing during the cutscenes. Will call ExecuteComplete once video is complete.
/// </summary>
public class CutSceneState : AGameState
{
    public CutSceneState()
    {

    }

    #region references
    #endregion

    #region handlers
    #endregion

    #region logic
    #endregion

    public override void Execute()
    {
        ExecuteComplete?.Invoke();
    }
    public override void Cancel()
    {
        CancelComplete?.Invoke();
    }
}