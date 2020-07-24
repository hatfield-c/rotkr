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

            // Update the data classes
            LayerSectionData section = data.LayerSectionDatum[data.CurrentSectionIndex];
            section.WasChosen = true;
            for(int i = 0; i < section.WyeDatum.Count; i++)
                if (section.WyeDatum[i] == chosenWye)
                    section.ChosenNodeIndex = i;

            data.CurrentSectionIndex++;
            if(data.CurrentSectionIndex >= data.SectionCount)
            {
                data.CurrentSectionIndex = data.SectionCount - 1;
                data.Completed = true;
            }

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