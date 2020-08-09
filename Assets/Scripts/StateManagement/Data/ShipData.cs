using System.Collections.Generic;

public class ShipData
{
    public ShipData()
    {
        HunkDatum = new List<HunkData>();
        RatDatum = new List<RatData>();
    }
    public List<HunkData> HunkDatum = null;
    public List<RatData> RatDatum = null;
    public int MaxRatCount = 5;
}