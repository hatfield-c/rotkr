using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairMenu : AView
{
    #region references
    [SerializeField] Button BTN_Confirm = null;

    [SerializeField] Button BTN_IncreaseHunkRepair = null;
    [SerializeField] Button BTN_DecreaseHunkRepair = null;
    [SerializeField] Button BTN_Repair = null;
    [SerializeField] List<RepairHunkCell> hunkCells = null;
    #endregion

    #region variables
    float hunkIntegrityRatio = 0;
    float newIntegrityRatio = 0;
    int cellsToFill = 0;
    int cellsToRepair = 0;
    #endregion


    new void Awake() { base.Awake(); }
    new void Start() { base.Start(); }
    public void Init(ShipData data, List<Hunk> deletedHunks, Action callback = null)
    {
        if (callback != null)
        {
            BTN_Confirm.onClick.AddListener(() =>
            {
                callback.Invoke();

                BTN_Confirm.onClick.RemoveAllListeners();
                BTN_Repair.onClick.RemoveAllListeners();
                BTN_IncreaseHunkRepair.onClick.RemoveAllListeners();
                BTN_DecreaseHunkRepair.onClick.RemoveAllListeners();

                BTN_Confirm.interactable = false;
                BTN_Repair.interactable = false;
                BTN_IncreaseHunkRepair.interactable = false;
                BTN_DecreaseHunkRepair.interactable = false;
            });
        }

        if(deletedHunks == null || deletedHunks.Count < 1)
        {
            BTN_Repair.interactable = false;
            BTN_IncreaseHunkRepair.interactable = false;
            BTN_DecreaseHunkRepair.interactable = false;
        }
        else
        {
            BTN_IncreaseHunkRepair.interactable = true;
            BTN_Repair.onClick.AddListener(() => HunkRepair(deletedHunks));
            BTN_IncreaseHunkRepair.onClick.AddListener(() => IncreaseHunkRepair(deletedHunks));
            BTN_DecreaseHunkRepair.onClick.AddListener(() => DecreaseHunkRepair(deletedHunks));
        }

        hunkIntegrityRatio = 1 - ((float)deletedHunks.Count / data.HunkDatum.Count);
        UpdateHunkRepairDisplay(hunkIntegrityRatio, true);
    }
    public override void Show()
    {
        base.Show();
    }
    public override void Hide()
    {
        base.Hide();
    }

    //protected void RepairAll(List<Hunk> deletedHunks)
    //{

    //    while(deletedHunks.Count > 0)
    //    {
    //        Hunk repairedHunk = deletedHunks[0].Repair();
    //        deletedHunks.Remove(repairedHunk);
    //    }

    //    BTN_RepairAll.interactable = false;
    //}

    void UpdateHunkRepairDisplay(float hunkRatio, bool instantUpdate = false)
    {
        cellsToFill = Mathf.FloorToInt(hunkCells.Count * hunkRatio);
        for(int i = 0; i < hunkCells.Count; i++)
        {
            if (i < cellsToFill)
                hunkCells[i].Fill(true);
            else
                hunkCells[i].Fill(false, instantUpdate);
        }
    }
    void HunkRepair(List<Hunk> deletedHunks)
    {

    }
    void IncreaseHunkRepair(List<Hunk> deletedHunks)
    {
        if(hunkIntegrityRatio < 1)
        {
            newIntegrityRatio = hunkIntegrityRatio + (1 * cellsToRepair / hunkCells.Count);

            BTN_DecreaseHunkRepair.interactable = true;
            BTN_Repair.interactable = true;

            if (newIntegrityRatio < 1)
            {
                cellsToRepair++;
                int highlightIndex = cellsToFill - 1 + cellsToRepair;
                hunkCells[highlightIndex].Highlight(true);
            }
            else
            {
                BTN_IncreaseHunkRepair.interactable = false;
            }
        }
        else
        {
            hunkIntegrityRatio = 1;
            BTN_IncreaseHunkRepair.interactable = false;
        }

    }
    void DecreaseHunkRepair(List<Hunk> deletedHunks)
    {
        if(hunkIntegrityRatio > 0)
        {
            newIntegrityRatio = hunkIntegrityRatio + (1 * cellsToRepair / hunkCells.Count);

            BTN_IncreaseHunkRepair.interactable = true;

            if(newIntegrityRatio >= hunkIntegrityRatio)
            {
                cellsToRepair--;
            }
            else
            {

            }
        }
        else
        {
            hunkIntegrityRatio = 0;
            BTN_DecreaseHunkRepair.interactable = false;
        }
    }

}
