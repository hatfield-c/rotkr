﻿using System.Collections.Generic;

public class ShipData
{
    public ShipData()
    {
        HunkDatum = new List<HunkData>();
        RatDatum = new List<RatData>();
        ScrapData = new ScrapData(3);
        UpgradeData = new UpgradeData();
    }
    public List<HunkData> HunkDatum = null;
    public List<RatData> RatDatum = null;
    public ScrapData ScrapData = null;
    public UpgradeData UpgradeData = null;
    int maxRatCount = 5;

    public int GetMaxRatCount()
    {
        return maxRatCount;
    }
}