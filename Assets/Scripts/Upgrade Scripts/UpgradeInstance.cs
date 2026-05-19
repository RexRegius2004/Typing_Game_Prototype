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

     public bool IsMaxLevel()
    {
        return currentLevel >= data.maxLevel;
    }

    public void LevelUp()
    {
        if (currentLevel < data.maxLevel)
        {
            currentLevel++;
        }
    }

    public string GetLevelText()
    {
        return "Lv. " + currentLevel + "/" + data.maxLevel;
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
        return Mathf.FloorToInt(data.wageMultipler * currentLevel);
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
