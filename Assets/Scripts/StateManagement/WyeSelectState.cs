using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyeSelectState : AGameState
{
    public WyeSelectState(int numberOfNodes, int branchRange, int numberOfSections)
    {
        //generateNodes(numberOfNodes, branchRange, numberOfSections);
    }
    #region variables

    #endregion

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
    #endregion

    #region private functions
    void generateNodes(int numberOfNodes, int branchRange, int numberOfSections)
    {
        if(numberOfSections < 2)
        {
            Debug.LogError("Not enough Sections for this WyeSelectState, exiting");
            return;
        }
        List<WyeData> data = generateWyeData(numberOfNodes);

        List<List<WyeData>> branchData = new List<List<WyeData>>();
        for(int i = 0; i < numberOfSections; i++)
        {
            // If this is the first node in the map, make 1 node no branches
            if(i == 0)
            {
                //branchData[0].Add();
            }
            // If this is the last node in the map, make 1 node no branches
            else if(i == numberOfSections - 1)
            {

            }
        }
    }
    List<WyeData> generateWyeData(int numberOfNodes)
    {
        List<WyeData> data = new List<WyeData>();
        for(int i = 0; i < numberOfNodes; i++)
        {
            TypeOfWye type = TypeOfWye.CollectionChamber;
            if (UnityEngine.Random.Range(0, 1f) > .5f)
                type = TypeOfWye.Spillway;
            data.Add(new WyeData { WyeType = type });
        }
        return data;
    }
    #endregion
}