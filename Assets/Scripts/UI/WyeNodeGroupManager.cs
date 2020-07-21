using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WyeNodeGroupManager : MonoBehaviour
{
    #region references
    [SerializeField] ToggleGroup wyeNodeToggleGroup = null;
    Toggle currentSelectedToggle { get { return wyeNodeToggleGroup.ActiveToggles().FirstOrDefault(); } }
    WyeNode currentSelectedWyeNode;
    List<WyeNode> wyeNodes = new List<WyeNode>();
    #endregion

    void Start()
    {
        //SelectToggle(1);
        Init();
    }

    #region public functions
    public void Init()
    {
        // TODO: spawn in the wye nodes as your children

        // Store the toggles
        wyeNodes = this.GetComponentsInChildren<WyeNode>().ToList<WyeNode>();

        // subscribe to when they're toggled on
        foreach(WyeNode node in wyeNodes)
        {
            node.WyeNodeSelected += nodeSelected;
        }
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
