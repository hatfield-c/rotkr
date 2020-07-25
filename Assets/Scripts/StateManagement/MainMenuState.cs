using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuState : AGameState
{
    public MainMenuState()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    ~MainMenuState()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region state variables
    public enum GameEntryPoint { NewGame, Continue };
    GameEntryPoint chosenGameEntryPoint = GameEntryPoint.NewGame;
    #endregion

    #region references
    MainMenuStateReferences refs;
    #endregion

    #region handlers
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update references
        refs = GameObject.FindObjectOfType<MainMenuStateReferences>();

        refs.BTN_NewGame.onClick.AddListener(() =>
        {
            chosenGameEntryPoint = GameEntryPoint.NewGame;
            UnsubscribeAll();
            ExecuteComplete?.Invoke();
        });
        refs.BTN_Quit.onClick.AddListener(() =>
        {
            UnsubscribeAll();
            CancelComplete?.Invoke();
        });
    }
    #endregion

    #region logic
    #endregion

    #region public functions
    public override void Execute()
    {
        Debug.Log("Execute called for MainMenuState");
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
    void UnsubscribeAll()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        refs.BTN_NewGame.onClick.RemoveAllListeners();
        refs.BTN_Quit.onClick.RemoveAllListeners();
    }
    #endregion
}