public class ScrapData
{
    public ScrapData(int initialScrapCount)
    {
        scrapCount = initialScrapCount;
    }
    int scrapCount;

    public int GetScrap()
    {
        return scrapCount;
    }
    public int UseScrap(int expendedScrap)
    {
        scrapCount -= expendedScrap;
        return scrapCount;
    }

}
