using System;
using UnityEngine;
using UnityEngine.UI;

public class RepairMenu : AView
{
    #region references
    [SerializeField] Button BTN_Confirm = null;
    #endregion

    new void Awake() { base.Awake(); }
    new void Start() { base.Start(); }
    public void Init(Action callback = null)
    {
        if (callback != null)
        {
            BTN_Confirm.onClick.AddListener(() =>
            {
                callback.Invoke();
                BTN_Confirm.onClick.RemoveAllListeners();
                BTN_Confirm.interactable = false;
            });
        }
    }
    public override void Show()
    {
        base.Show();
    }
    public override void Hide()
    {
        base.Hide();
    }
}
