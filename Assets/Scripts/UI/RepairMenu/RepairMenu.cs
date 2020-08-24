using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairMenu : AView
{
    #region references
    [SerializeField] Button btn_Confirm = null;
    [SerializeField] Button btn_IncreaseHunkRepair = null;
    [SerializeField] Button btn_DecreaseHunkRepair = null;
    [SerializeField] Button btn_Repair = null;
    [SerializeField] Button btn_Recruit = null;
    [SerializeField] Button btn_UpgradeHardenedGlue = null;
    [SerializeField] Button btn_UpgradeReloadSpeed = null;
    [SerializeField] List<RepairHunkCell> hunkCells = null;
    [SerializeField] TextMeshProUGUI ratCountDisplay = null;
    [SerializeField] TextMeshProUGUI repairPriceTag = null;
    [SerializeField] TextMeshProUGUI recruitPriceTag = null;
    [SerializeField] TextMeshProUGUI hardenedGluePriceTag = null;
    [SerializeField] TextMeshProUGUI reloadSpeedPriceTag = null;
    #endregion

    #region variables
    public int PricePerRepairCell = 7;
    public int PricePerRecruit = 50;
    public int PricePerHardenedGlueUpgrade = 30;
    public int PricePerReloadSpeedUpgrade = 30;
    ShipData data;

    float hunkIntegrityRatio = 0;
    float newIntegrityRatio = 0;
    int cellsToFill = 0;
    int cellsToRepair = 0;

    
    #endregion


    new void Awake() { base.Awake(); }
    new void Start() { base.Start(); }
    public void Init(ShipData data, List<Hunk> deletedHunks, Action callback = null)
    {
        this.data = data;
        if (callback != null)
        {
            btn_Confirm.onClick.AddListener(() =>
            {
                callback.Invoke();

                btn_Confirm.onClick.RemoveAllListeners();
                btn_Repair.onClick.RemoveAllListeners();
                btn_IncreaseHunkRepair.onClick.RemoveAllListeners();
                btn_DecreaseHunkRepair.onClick.RemoveAllListeners();
                btn_Recruit.onClick.RemoveAllListeners();
                btn_UpgradeHardenedGlue.onClick.RemoveAllListeners();
                btn_UpgradeReloadSpeed.onClick.RemoveAllListeners();

                btn_Confirm.interactable = false;
                btn_Repair.interactable = false;
                btn_IncreaseHunkRepair.interactable = false;
                btn_DecreaseHunkRepair.interactable = false;
                btn_Recruit.interactable = false;
                btn_UpgradeHardenedGlue.interactable = false;
                btn_UpgradeReloadSpeed.interactable = false;
            });
        }

        // Repair
        if(deletedHunks == null || deletedHunks.Count < 1)
        {
            btn_Repair.interactable = false;
            btn_IncreaseHunkRepair.interactable = false;
            btn_DecreaseHunkRepair.interactable = false;
        }
        else
        {
            btn_IncreaseHunkRepair.interactable = true;
            btn_Repair.onClick.AddListener(() => hunkRepair(deletedHunks));
            btn_IncreaseHunkRepair.onClick.AddListener(() => increaseHunkRepair());
            btn_DecreaseHunkRepair.onClick.AddListener(() => decreaseHunkRepair());
        }

        hunkIntegrityRatio = 1 - ((float)deletedHunks.Count / data.HunkDatum.Count);
        updateHunkRepairDisplay(hunkIntegrityRatio, true);

        // Recruitment
        if (canRecruit())
        {
            btn_Recruit.interactable = true;
            btn_Recruit.onClick.AddListener(() =>
            {
                addRat();
                btn_Recruit.interactable = canRecruit();
            });
        }
        else
        {
            btn_Recruit.interactable = false;
        }
        updateRatCountDisplay();
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
        repairPriceTag.text = (cellsToRepair * PricePerRepairCell).ToString();
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
            if (isThisRepairAffordable(cellsToRepair + 1))
            {
                cellsToRepair++;
                updateRepairPriceTag(cellsToRepair);
            }
        }
        else
        {
            hunkIntegrityRatio = 1;
            btn_IncreaseHunkRepair.interactable = false;
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
            btn_Repair.interactable = true;
            btn_DecreaseHunkRepair.interactable = true;
        }
        else
        {
            cellsToRepair = 0;
            btn_Repair.interactable = false;
            btn_DecreaseHunkRepair.interactable = false;
        }

        newIntegrityRatio = hunkIntegrityRatio + ((float)cellsToRepair / hunkCells.Count);
        btn_IncreaseHunkRepair.interactable = (newIntegrityRatio < 1);
        btn_Repair.interactable = (hunkIntegrityRatio < 1);
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

    bool isThisRepairAffordable(int cellsToRepair)
    {
        if (cellsToRepair * PricePerRepairCell > data.ScrapData.GetScrap())
            return false;
        else
            return true;
    }

    bool canRecruit()
    {
        return data.RatDatum.Count < data.GetMaxRatCount();
    }
    bool canAffordRecruit()
    {
        return data.ScrapData.GetScrap() >= PricePerRecruit;
    }
    bool addRat()
    {
        if (!canRecruit() || !canAffordRecruit()) return false;

        string name = $"Joe {data.RatDatum.Count}";
        data.RatDatum.Add(new RatData(name));
        data.ScrapData.UseScrap(PricePerRecruit);
        updateRatCountDisplay();
        return true;
    }

    void updateRatCountDisplay()
    {
        ratCountDisplay.text = data.RatDatum.Count.ToString();
        updateRecruitPriceTag();
    }

    void updateRecruitPriceTag()
    {
        recruitPriceTag.text = PricePerRecruit.ToString();
    }
}
