using System;

public class UpgradeData
{
    public UpgradeData()
    {
        hardenedGlueLevel = minHardenedGlueLevel;
        reloadSpeedLevel = minReloadSpeedLevel;
    }

    int hardenedGlueLevel = 0;
    int minHardenedGlueLevel = 0;
    int maxHardenedGlueLevel = 10;
    public float BaseBreakForce = 1500;
    public Action<float> GlueBreakForceUpdated;
    public int HardenedGlueLevel
    {
        get { return hardenedGlueLevel; }
        set 
        {
            value = (value > maxHardenedGlueLevel) ? maxHardenedGlueLevel : value;
            value = (value < minHardenedGlueLevel) ? minHardenedGlueLevel : value;
            hardenedGlueLevel = value;
            GlueBreakForceUpdated?.Invoke(GetNewBreakForce());
        }
    }
    public float GetNewBreakForce()
    {
        return BaseBreakForce + (((float)hardenedGlueLevel / maxHardenedGlueLevel) * BaseBreakForce);
    }
    public bool CanUpgradeGlue()
    {
        if (hardenedGlueLevel >= maxHardenedGlueLevel) return false;
        return true;
    }

    int reloadSpeedLevel = 0;
    int minReloadSpeedLevel = 0;
    int maxReloadSpeedLevel = 10;
    public Action<float> ReloadSpeedRatioUpdated;
    public int ReloadSpeedLevel
    {
        get { return reloadSpeedLevel; }
        set
        {
            value = (value > maxReloadSpeedLevel) ? maxReloadSpeedLevel : value;
            value = (value < minReloadSpeedLevel) ? minReloadSpeedLevel : value;
            reloadSpeedLevel = value;
            ReloadSpeedRatioUpdated?.Invoke(GetNewReloadRatio());
        }    
    }
    public float GetNewReloadRatio()
    {
        return 1 - ((float) reloadSpeedLevel / maxReloadSpeedLevel);
    }
    public bool CanUpgradeReload()
    {
        if (reloadSpeedLevel >= maxReloadSpeedLevel) return false;
        return true;
    }
}
