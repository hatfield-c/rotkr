using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : AGameState
{
    public MainMenuState()
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
