using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LayerMapState : AGameState
{
    // Create a new random LayerMap with the WyeSelectState
    public LayerMapState(int numberOfSections, int branchRange)
    {
        this.numberOfSections = numberOfSections;
        this.branchRange = branchRange;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public LayerMapState(LayerMapData data)
    {
    }
    ~LayerMapState()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region variables
    int numberOfSections;
    int branchRange;
    #endregion

    #region references
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
        //refs.wyeNodeGroupManager.Init(new List<WyeData>() { new WyeData { WyeType = TypeOfWye.CollectionChamber }, new WyeData { WyeType = TypeOfWye.Spillway } });
        List<List<WyeData>> sectionDatum = generateNodes(numberOfSections, branchRange);
        if(sectionDatum == null)
        {
            Debug.LogError("Not enough sections");
            return;
        }
        refs.wyeNodeGroupManager.Init(sectionDatum);
        refs.BTN_Go.onClick.AddListener(() =>
        {
            chosenWye = refs.wyeNodeGroupManager.GetSelectedWyeData();
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

    #region private functions
    List<List<WyeData>> generateNodes(int numberOfSections, int branchRange)
    {
        if(numberOfSections < 2)
        {
            Debug.LogError("Not enough Sections for this WyeSelectState, exiting");
            return null;
        }

        List<List<WyeData>> sectionData = new List<List<WyeData>>();
        
        for(int i = 0; i < numberOfSections; i++)
        {
            // If this is the first or last section in the map, make 1 node no branches
            if(i == 0 || i == numberOfSections - 1)
            {
                List<WyeData> newSoloSection = new List<WyeData>();
                newSoloSection.Add(getRandomWyeData());
                sectionData.Add(newSoloSection);
                continue;
            }
            
            // Otherwise create some nodes for this section, given it is valid with the remaining nodes
            int randomSectionNodeNumber = UnityEngine.Random.Range(1, (branchRange + 1));

            List<WyeData> newSection = new List<WyeData>();
            for (int j = 0; j < randomSectionNodeNumber; j++)
                newSection.Add(getRandomWyeData());
            sectionData.Add(newSection);
        }

        return sectionData;
    }
    WyeData getRandomWyeData()
    {
        TypeOfWye type = TypeOfWye.CollectionChamber;
        if (UnityEngine.Random.Range(0, 1f) > .5f)
            type = TypeOfWye.Spillway;
        return new WyeData { WyeType = type };
    }
    #endregion
}

// TODO: move this to external file
public class LayerMapData { }
