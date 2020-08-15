﻿using System;
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
        Debug.Log($"UpdateHunkRepairDisplay, cellsToFill: {cellsToFill}");
        for(int i = 0; i < hunkCells.Count; i++)
        {
            if (i < cellsToFill)
            {
                hunkCells[i].Fill(true);
            }
            else
            {
                hunkCells[i].Fill(false);
            }
        }
    }
    void HunkRepair(List<Hunk> deletedHunks)
    {
        hunkIntegrityRatio = newIntegrityRatio;
        cellsToRepair = 0;
        for (int i = 0; i < cellsToRepair; i++)
        {
            Hunk repairedHunk = deletedHunks[0].Repair();
            deletedHunks.Remove(repairedHunk);
        }
        UpdateHunkRepairDisplay(hunkIntegrityRatio);
        UpdateRepairButtons();
        //HighlightCellsToRepair();
    }
    void IncreaseHunkRepair(List<Hunk> deletedHunks)
    {
        if(hunkIntegrityRatio < 1)
        {
            cellsToRepair++;
        }
        else
        {
            hunkIntegrityRatio = 1;
            BTN_IncreaseHunkRepair.interactable = false;
        }
        UpdateRepairButtons();
    }
    void DecreaseHunkRepair(List<Hunk> deletedHunks)
    {
        cellsToRepair--;
        UpdateRepairButtons();
    }

    void UpdateRepairButtons()
    {
        if (cellsToRepair > 0)
        {
            BTN_Repair.interactable = true;
            BTN_DecreaseHunkRepair.interactable = true;
        }
        else
        {
            cellsToRepair = 0;
            BTN_Repair.interactable = false;
            BTN_DecreaseHunkRepair.interactable = false;
        }

        newIntegrityRatio = hunkIntegrityRatio + ((float)cellsToRepair / hunkCells.Count);
        BTN_IncreaseHunkRepair.interactable = (newIntegrityRatio < 1);
        BTN_Repair.interactable = (hunkIntegrityRatio < 1);
        Debug.Log($"newIntegrityRatio: {newIntegrityRatio}, hunkIntegrityRatio: {hunkIntegrityRatio}");

        HighlightCellsToRepair();
    }
    void HighlightCellsToRepair()
    {
        int highlightStartIndex = cellsToFill;

        for (int i = highlightStartIndex; i < hunkCells.Count; i++)
        {
            if (i < highlightStartIndex + cellsToRepair)
                hunkCells[i].Highlight();
            else
                hunkCells[i].Fill(false);
        }
    }
}