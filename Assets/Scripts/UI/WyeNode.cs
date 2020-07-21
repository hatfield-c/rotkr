using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WyeNode : MonoBehaviour
{
    WyeData data;
    public Action<WyeNode> WyeNodeSelected;

    #region references
    [SerializeField] Toggle toggle = null;
    #endregion

    public void Init(WyeData data)
    {
        this.data = data;
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
    #endregion
}
