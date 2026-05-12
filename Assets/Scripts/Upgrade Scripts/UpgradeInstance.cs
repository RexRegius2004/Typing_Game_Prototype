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

    public float GetCritChanceBonus()
    {
        return data.critChance * currentLevel;
    }

    public float GetCritHitBonus()
    {
        return data.critHit * currentLevel;
    }

    
}
