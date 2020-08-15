using System.Collections.Generic;

public class ShipData
{
    public ShipData()
    {
        HunkDatum = new List<HunkData>();
        RatDatum = new List<RatData>();
        ScrapData = new ScrapData(3);
    }
    public List<HunkData> HunkDatum = null;
    public List<RatData> RatDatum = null;
    public ScrapData ScrapData = null;
    public int MaxRatCount = 5;
}