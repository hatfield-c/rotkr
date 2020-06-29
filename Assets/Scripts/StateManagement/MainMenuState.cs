using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : AGameState
{
    public enum GameEntryPoint { NewGame, Continue};
    GameEntryPoint chosenGameEntryPoint = GameEntryPoint.NewGame;
    public MainMenuState()
    {

    }

    #region references
    #endregion

    #region handlers
    #endregion

    #region logic
    #endregion

    #region public functions
    public override void Execute()
    {
        ExecuteComplete?.Invoke();
    }
    public override void Cancel()
    {
        CancelComplete?.Invoke();
    }
    public GameEntryPoint ChosenGameEntryPoint()
    {
        return chosenGameEntryPoint;
    }
    #endregion

    #region private functions
    #endregion
}