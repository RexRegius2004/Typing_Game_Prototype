using UnityEngine;
using System.Collections.Generic;
using System.Data.Common;

public class UpgradeManager : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseCritChance = 0;
    public float baseCritHit = 1;
    public int aheadOfSchduleThreshhold = 30;
    

    [Header("Current Stats")]
    public float currentCritChance;
    public float currentCritHit;
    public int currentWageBonus = 0;
    public int currentAheadSchedBonus = 0;
    public int currentDelayBonus = 0;
    public float currentconsistencymultiplier = 0;

    public TimerScript timerScript;
    

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
            currentCritChance += upgrade.GetCritChance();
            currentCritHit += upgrade.GetCritHit();  

            switch(upgrade.data.upgradeName)
            {
                case ("Wage"):
                currentWageBonus = upgrade.GetWageBonus();
                break;

                case ("Ahead of Schedule"):
                currentAheadSchedBonus = upgrade.GetAheadSchedBonus();
                break;

                case ("Delay Tactics"):
                currentDelayBonus = upgrade.GetDelayBonus();
                break;

                case ("Consistency Bonus"):
                currentconsistencymultiplier = upgrade.GetConsistencyBonus();
                break;
            }
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



    public bool HasUpgrade(string upgradeName)
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.data.upgradeName == upgradeName)
            {
                return true;
            }
        }

        return false;
    }

    public int AheadofSchedule()
    {
        if (aheadOfSchduleThreshhold < timerScript.GetRemainingTime())
        return currentAheadSchedBonus;
        else
        return 0;
    }

    public float ConsistencyBonus()
    {
        return PlayerPrefs.GetInt("WinStreak") * currentconsistencymultiplier;
    }
}
