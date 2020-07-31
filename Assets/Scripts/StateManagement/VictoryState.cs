using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryState : AGameState
{
    public VictoryState(InputMaster controls)
    {
        this.controls = controls;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    ~VictoryState()
    {
        UnsubscribeAll();
    }

    #region variables
    #endregion

    #region references
    VictoryStateReferences refs;
    InputMaster controls;
    #endregion

    #region handlers
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update references
        refs = Object.FindObjectOfType<VictoryStateReferences>();
    }
    #endregion

    #region logic
    #endregion

    #region public functions
    public override void Execute()
    {
        Debug.Log(refs);
        // Setup
        refs.BTN_BackToMainMenu.onClick.AddListener(() =>
        {
            refs.BTN_BackToMainMenu.onClick.RemoveAllListeners();
            UnsubscribeAll();
            ExecuteComplete?.Invoke();
        });
    }
    public override void Cancel()
    {
        UnsubscribeAll();
        CancelComplete?.Invoke();
    }
    #endregion

    #region private functions
    void UnsubscribeAll()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

}
