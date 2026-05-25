using UnityEngine;

public class UpgradeInstance
{
    public UpgradeData data;
    public int currentLevel;

    public UpgradeInstance(UpgradeData data)
    {
        this.data = data;
        currentLevel = 1;
    }

    public void LevelUp()
    {
            currentLevel++;
    }

    public string GetLevelText()
    {
        return "Lv. " + currentLevel;
    }

    public float GetCritChance()
    {
        return data.critChance * currentLevel;
    }

    public float GetCritHit()
    {
        return data.critHit * currentLevel;
    }

    public int GetWageBonus()
    {
        return data.wageBonus * currentLevel;
    }
    
    public int GetAheadSchedBonus()
    {
        return Mathf.FloorToInt(data.aheadSchedMultiplier * currentLevel);
    }

    public int GetDelayBonus()
    {
        return data.delaytacticbonus * currentLevel;
    }

    public float GetConsistencyBonus()
    {
        return data.consistencyBonusMultiplier * currentLevel;
    }
    
}
