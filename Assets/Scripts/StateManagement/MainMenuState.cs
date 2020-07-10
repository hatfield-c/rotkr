using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : AGameState
{
    #region references
    MainMenuUI MainMenuUI;
    #endregion

    #region state variables
    public enum GameEntryPoint { NewGame, Continue };
    GameEntryPoint chosenGameEntryPoint = GameEntryPoint.NewGame;
    #endregion

    public MainMenuState(MainMenuUI ui)
    {
        MainMenuUI = ui;
        ui.BTN_NewGame.onClick.AddListener(() => 
        {
            chosenGameEntryPoint = GameEntryPoint.NewGame;
            ExecuteComplete?.Invoke();
        });
        ui.BTN_Quit.onClick.AddListener(() => 
        {
            CancelComplete?.Invoke();
        });
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