using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseCritChance = 0;
    public float baseCritHit = 1;
    public float baseWageMultiplier = 1;

    [Header("Percentage")]
    public float currentCritChance;
    public float currentCritHit;
    public float currentWageMultiplier;

    [Header("Upgrade Bonuses")]
    public int DelayTacticsBonus;
    public int ConsistencyBonus;
    public int PerfectAccuracyBonus;
    public int HighAccuracyBonus;
    private int AHeadOfScheduleBonus;
    
    public List<UpgradeInstance> upgrades = new List<UpgradeInstance>();

    void Start()
    {
        RecalculateStats();
    }

    public void AddUpgrade(UpgradeData upgradeData)
{
    UpgradeInstance existing = upgrades.Find(u => u.data == upgradeData);

    if (existing != null)
    {
        if (!existing.IsMaxLevel())
        {
            existing.LevelUp();
        }
        else
        {
            Debug.Log(upgradeData.upgradeName + " is already MAX level!");
            return;
        }
    }
    else
    {
        upgrades.Add(new UpgradeInstance(upgradeData));
    }

    RecalculateStats();
    DebugUpgrades();
}
    public void RecalculateStats()
    {
        // Reset to base
        currentCritChance = baseCritChance;
        currentCritHit = baseCritHit;

        foreach (var upgrade in upgrades)
        {
            currentCritChance += upgrade.GetCritChanceBonus();
            currentCritHit += upgrade.GetCritHitBonus();
        }

        Debug.Log("CritHit: " + currentCritHit + 
          " CritChance: " + currentCritChance);
    }

    public void ResetUpgrades()
    {
        upgrades.Clear();
        RecalculateStats();
        Debug.Log("All upgrades have been reset.");
    }

    void DebugUpgrades()
{
    foreach (var upgrade in upgrades)
    {
        Debug.Log(upgrade.data.upgradeName + " - " + upgrade.GetLevelText());
    }
}
}
