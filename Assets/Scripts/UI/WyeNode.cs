using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WyeNode : MonoBehaviour
{
    WyeData data;
    public Action<WyeNode> WyeNodeSelected;

    #region references
    [SerializeField] Toggle toggle = null;
    [SerializeField] TextMeshProUGUI label = null;
    #endregion

    public void Init(WyeData data)
    {
        this.data = data;
        label.text = data.WyeType.ToString() + $" ID: {data.ID}";
    }

    #region handlers
    void OnDisable()
    {
        WyeNodeSelected = null;
    }
    public void OnToggle(bool value)
    {
        if (value)
        {
            WyeNodeSelected?.Invoke(this);
        }
    }
    #endregion

    #region public functions
    public WyeData GetData()
    {
        return data;
    }
    
    public void DisplayHistory(bool wasChosen)
    {
        // Change how this node is displayed
        if (wasChosen)
        {
            label.text = "[" + label.text + "]";
        }
        else
        {
            label.text = "-" + label.text + "-";
        }
    }
    public void ToggleInteractable(bool toggle)
    {
        this.toggle.interactable = toggle;
    }
    #endregion
}
