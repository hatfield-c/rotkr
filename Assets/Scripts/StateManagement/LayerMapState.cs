using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LayerMapState : AGameState
{
    /// <summary>
    /// Recreate from data 
    /// </summary>
    /// <param name="data"></param>
    public LayerMapState(LayerMapData data)
    {
        this.data = data;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    ~LayerMapState()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region references
    LayerMapData data;
    LayerMapStateReferences refs;
    WyeData chosenWye;
    #endregion

    #region handlers
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        refs = GameObject.FindObjectOfType<LayerMapStateReferences>();
    }
    #endregion

    #region logic
    #endregion

    #region public functions
    public override void Execute()
    {
        // Setup
        refs.wyeNodeGroupManager.Init(data);
        refs.BTN_Go.onClick.AddListener(() =>
        {
            chosenWye = refs.wyeNodeGroupManager.GetSelectedWyeData();

            // TODO: REFACTOR THIS
            // Update the data classes
            //LayerSectionData section = data.LayerSectionDatum[data.CurrentSectionIndex];
            //for(int i = 0; i < section.WyeDatum.Count; i++)
            //{
            //    if (section.WyeDatum[i] == chosenWye)
            //    {
            //        section.ChooseNode(i);
            //        break;
            //    }
            //}

            refs.BTN_Go.onClick.RemoveAllListeners();
            ExecuteComplete?.Invoke();
        });
    }
    public override void Cancel()
    {
        CancelComplete?.Invoke();
    }

    public WyeData ChosenWye()
    {
        return chosenWye;
    }
    #endregion
}