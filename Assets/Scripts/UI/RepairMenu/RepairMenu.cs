using System;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] TextMeshProUGUI RepairPriceTag = null;
    #endregion

    #region variables
    public int PricePerRepairCell = 7;

    float hunkIntegrityRatio = 0;
    float newIntegrityRatio = 0;
    int cellsToFill = 0;
    int cellsToRepair = 0;

    ShipData data;
    #endregion


    new void Awake() { base.Awake(); }
    new void Start() { base.Start(); }
    public void Init(ShipData data, List<Hunk> deletedHunks, Action callback = null)
    {
        this.data = data;
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
            BTN_Repair.onClick.AddListener(() => hunkRepair(deletedHunks));
            BTN_IncreaseHunkRepair.onClick.AddListener(() => increaseHunkRepair());
            BTN_DecreaseHunkRepair.onClick.AddListener(() => decreaseHunkRepair());
        }

        hunkIntegrityRatio = 1 - ((float)deletedHunks.Count / data.HunkDatum.Count);
        updateHunkRepairDisplay(hunkIntegrityRatio, true);
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

    void updateRepairPriceTag(int cellsToRepair)
    {
        RepairPriceTag.text = (cellsToRepair * PricePerRepairCell).ToString();
    }

    void updateHunkRepairDisplay(float hunkRatio, bool instantUpdate = false)
    {
        cellsToFill = Mathf.FloorToInt(hunkCells.Count * hunkRatio);
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
        updateRepairPriceTag(cellsToRepair);
    }
    void hunkRepair(List<Hunk> deletedHunks)
    {
        int hunksToRepair = Mathf.CeilToInt((float)cellsToRepair / hunkCells.Count * data.HunkDatum.Count);
        hunksToRepair = (hunksToRepair > deletedHunks.Count) ? deletedHunks.Count : hunksToRepair;
        for (int i = 0; i < hunksToRepair; i++)
        {
            Hunk repairedHunk = deletedHunks[0].Repair();
            deletedHunks.Remove(repairedHunk);
        }

        // Use the scrap to repair
        data.ScrapData.UseScrap(cellsToRepair * PricePerRepairCell);

        // Update Displays
        hunkIntegrityRatio = newIntegrityRatio;
        cellsToRepair = 0;
        updateHunkRepairDisplay(hunkIntegrityRatio);
        updateRepairButtons();
    }
    void increaseHunkRepair()
    {
        if(hunkIntegrityRatio < 1)
        {
            // If we can afford it
            if (IsThisAffordable(cellsToRepair + 1))
            {
                cellsToRepair++;
                updateRepairPriceTag(cellsToRepair);
            }
        }
        else
        {
            hunkIntegrityRatio = 1;
            BTN_IncreaseHunkRepair.interactable = false;
        }
        updateRepairButtons();
    }
    void decreaseHunkRepair()
    {
        cellsToRepair--;
        updateRepairPriceTag(cellsToRepair);
        updateRepairButtons();
    }

    void updateRepairButtons()
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
        //Debug.Log($"newIntegrityRatio: {newIntegrityRatio}, hunkIntegrityRatio: {hunkIntegrityRatio}");

        highlightCellsToRepair();
    }
    void highlightCellsToRepair()
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

    bool IsThisAffordable(int cellsToRepair)
    {
        if (cellsToRepair * PricePerRepairCell > data.ScrapData.GetScrap())
            return false;
        else
            return true;
    }
}
