using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class WyeNodeGroupManager : MonoBehaviour
{
    public Action<WyeNode> NodeSelected;

    #region references
    [SerializeField] ToggleGroup wyeNodeToggleGroup = null;
    [SerializeField] GameObject wyeNodePrefab = null;
    [SerializeField] GameObject layerSectionPrefab = null;
    [SerializeField] Transform sectionHolder = null;
    Toggle currentSelectedToggle { get { return wyeNodeToggleGroup.ActiveToggles().FirstOrDefault(); } }
    WyeNode currentSelectedWyeNode;
    List<WyeNode> wyeNodes = new List<WyeNode>();
    #endregion

    void Start()
    {
        //SelectToggle(1);
        //Init(new List<WyeData>() { new WyeData { WyeType = TypeOfWye.CollectionChamber }, new WyeData { WyeType = TypeOfWye.Spillway } });
    }

    #region public functions
    public void Init(LayerMapData layerData)
    {
        foreach(LayerSectionData sectionData in layerData.LayerSectionDatum)
        {
            // Create each section and pass the data it needs.
            GameObject section = Instantiate(layerSectionPrefab, sectionHolder);
            bool isCurrentSection = section.transform.GetSiblingIndex() == layerData.CurrentSectionIndex;
            section.GetComponent<LayerSection>().Init(sectionData, wyeNodePrefab, wyeNodeToggleGroup, isCurrentSection);
        }

        // Store the toggles.
        wyeNodes = GetComponentsInChildren<WyeNode>().ToList();

        // Subscribe to when they're toggled on.
        foreach(WyeNode node in wyeNodes)
            node.WyeNodeSelected += nodeSelected;

        // Allow switch off to false to prevent weird edge case.
        Sequence sequence = DOTween.Sequence();
        sequence.InsertCallback(.1f, () => wyeNodeToggleGroup.allowSwitchOff = false);
    }

    public void SelectToggle(int id)
    {
        var toggles = wyeNodeToggleGroup.GetComponentsInChildren<Toggle>();
        toggles[id].isOn = true;
    }

    public WyeData GetSelectedWyeData()
    {
        return currentSelectedWyeNode.GetData();
    }
    #endregion

    #region private functions
    void nodeSelected(WyeNode node)
    {
        currentSelectedWyeNode = node;
        NodeSelected?.Invoke(node);
    }
    #endregion
}
