using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WyeNodeGroupManager : MonoBehaviour
{
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
            // Create each section
            GameObject section = Instantiate(layerSectionPrefab, sectionHolder);
            foreach(WyeData data in sectionData.WyeDatum)
            {
                // Create each node and attach it to the section
                GameObject GO = Instantiate(wyeNodePrefab, section.transform);
                WyeNode node = GO.GetComponent<WyeNode>();
                node.Init(data);
                node.GetComponent<Toggle>().group = wyeNodeToggleGroup;
            }

            // Modify the section if it was in the past
            if (sectionData.WasChosen)
            {
                section.GetComponent<Image>().enabled = false;
            }
        }

        // Store the toggles
        wyeNodes = GetComponentsInChildren<WyeNode>().ToList();

        // subscribe to when they're toggled on
        foreach(WyeNode node in wyeNodes)
            node.WyeNodeSelected += nodeSelected;

        // Allow switch off to false to prevent weird edge case
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
        Debug.Log($"node selected: {node.name}");
    }
    #endregion
}
