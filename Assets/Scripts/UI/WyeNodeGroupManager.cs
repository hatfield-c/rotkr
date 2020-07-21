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
    [SerializeField] Transform tempNodeHolder = null;
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
    public void Init(List<WyeData> datum)
    {
        // TODO: spawn in the wye nodes as your children
        foreach(WyeData data in datum)
        {
            GameObject GO = Instantiate(wyeNodePrefab, tempNodeHolder);
            WyeNode node = GO.GetComponent<WyeNode>();
            node.Init(data);
            node.GetComponent<Toggle>().group = wyeNodeToggleGroup;
        }

        // Store the toggles
        wyeNodes = GetComponentsInChildren<WyeNode>().ToList();

        

        // subscribe to when they're toggled on
        foreach(WyeNode node in wyeNodes)
        {
            node.WyeNodeSelected += nodeSelected;
        }

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
