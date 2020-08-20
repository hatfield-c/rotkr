using System;

public class ScrapData
{
    public ScrapData(int initialScrapCount)
    {
        scrapCount = initialScrapCount;
    }
    public Action ScrapUpdated;
    int scrapCount;

    public int GetScrap()
    {
        return scrapCount;
    }
    public int AddScrap(int value)
    {
        scrapCount += value;
        ScrapUpdated?.Invoke();
        return scrapCount;
    }
    public int UseScrap(int expendedScrap)
    {
        scrapCount -= expendedScrap;
        ScrapUpdated?.Invoke();
        return scrapCount;
    }
}
