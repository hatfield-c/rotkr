using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairMenu : AView
{
    #region references
    [SerializeField] Button BTN_Confirm = null;
    [SerializeField] Button BTN_RepairAll = null;
    #endregion

    new void Awake() { base.Awake(); }
    new void Start() { base.Start(); }
    public void Init(Action callback = null, List<Hunk> deletedHunks = null)
    {
        if (callback != null)
        {
            BTN_Confirm.onClick.AddListener(() =>
            {
                callback.Invoke();

                BTN_Confirm.onClick.RemoveAllListeners();
                BTN_RepairAll.onClick.RemoveAllListeners();

                BTN_Confirm.interactable = false;
                BTN_RepairAll.interactable = false;
            });
        }

        if(deletedHunks == null || deletedHunks.Count < 1){
            BTN_RepairAll.interactable = false;
        } else {
            BTN_RepairAll.onClick.AddListener(() => RepairAll(deletedHunks));
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

    protected void RepairAll(List<Hunk> deletedHunks){

        while(deletedHunks.Count > 0){
            Hunk repairedHunk = deletedHunks[0].Repair();
            deletedHunks.Remove(repairedHunk);
        }

        BTN_RepairAll.interactable = false;
    }
}
