using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuState : AGameState
{
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
    ~MainMenuState()
    {

    }

    #region state variables
    public enum GameEntryPoint { NewGame, Continue };
    GameEntryPoint chosenGameEntryPoint = GameEntryPoint.NewGame;
    #endregion

    #region references
    MainMenuUI MainMenuUI;
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
        if (SceneManager.GetActiveScene().name == "Master")
            return;

        //SceneManager.LoadScene("Master");
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