using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseCritChance = 0;
    public float baseCritHit = 1;
    public int aheadOfScheduleThreshhold = 30;
    

    [Header("Current Stats")]
    public float currentCritChance;
    public float currentCritHit;

    public int currentWageBonus = 0;
    public int currentAheadSchedBonus = 0;
    public int currentDelayBonus = 0;

    public float currentconsistencymultiplier = 0;

    [Header("References")]
    public TimerScript timerScript;

    [Header("Upgrade Database")]
    public UpgradeData[] allUpgradeData;

    [Header("Owned Upgrades")]
    public List<UpgradeInstance> upgrades =
        new List<UpgradeInstance>();

    void Start()
    {
        LoadUpgrades();
    }

    // =====================================
    // ADD / LEVEL UP UPGRADE
    // =====================================

    public void AddUpgrade(
        UpgradeData upgradeData
    )
    {
        UpgradeInstance existing =
            upgrades.Find(
                u => u.data == upgradeData
            );

        if (existing != null)
        {
            if (!existing.IsMaxLevel())
            {
                existing.LevelUp();
            }
            else
            {
                Debug.Log(
                    upgradeData.upgradeName +
                    " is already MAX level!"
                );

                return;
            }
        }
        else
        {
            upgrades.Add(
                new UpgradeInstance(upgradeData)
            );
        }

        RecalculateStats();

        SaveUpgrades();

        DebugUpgrades();
    }

    // =====================================
    // RECALCULATE ALL STATS
    // =====================================

    public void RecalculateStats()
    {
        // RESET TO BASE

        currentCritChance =
            baseCritChance;

        currentCritHit =
            baseCritHit;

        currentWageBonus = 0;
        currentAheadSchedBonus = 0;
        currentDelayBonus = 0;
        currentconsistencymultiplier = 0;

        // CALCULATE

        foreach (var upgrade in upgrades)
        {
            currentCritChance +=
                upgrade.GetCritChance();

            currentCritHit +=
                upgrade.GetCritHit();

            switch (
                upgrade.data.upgradeName
            )
            {
                case "Wage":

                    currentWageBonus =
                        upgrade.GetWageBonus();

                    break;

                case "Ahead of Schedule":

                    currentAheadSchedBonus =
                        upgrade.GetAheadSchedBonus();

                    break;

                case "Delay Tactics":

                    currentDelayBonus =
                        upgrade.GetDelayBonus();

                    break;

                case "Consistency Bonus":

                    currentconsistencymultiplier =
                        upgrade.GetConsistencyBonus();

                    break;
            }
        }

        Debug.Log(
            "CritHit: " +
            currentCritHit +
            " | CritChance: " +
            currentCritChance
        );
    }

    // =====================================
    // SAVE UPGRADES
    // =====================================

    public void SaveUpgrades()
    {
        string saveData = "";

        foreach (var upgrade in upgrades)
        {
            saveData +=
                upgrade.data.upgradeName +
                "|" +
                upgrade.currentLevel +
                ";";
        }

        PlayerPrefs.SetString(
            "UPGRADES",
            saveData
        );

        PlayerPrefs.Save();

        Debug.Log(
            "Upgrades Saved: " +
            saveData
        );
    }

    // =====================================
    // LOAD UPGRADES
    // =====================================

    public void LoadUpgrades()
    {
        if (!PlayerPrefs.HasKey("UPGRADES"))
        {
            Debug.Log(
                "No upgrade save found."
            );

            RecalculateStats();

            return;
        }

        upgrades.Clear();

        string saveData =
            PlayerPrefs.GetString(
                "UPGRADES"
            );

        string[] savedUpgrades =
            saveData.Split(';');

        foreach (string entry in savedUpgrades)
        {
            if (
                string.IsNullOrEmpty(entry)
            )
                continue;

            string[] parts =
                entry.Split('|');

            string upgradeName =
                parts[0];

            int level =
                int.Parse(parts[1]);

            UpgradeData foundData =
                FindUpgradeDataByName(
                    upgradeName
                );

            if (foundData != null)
            {
                UpgradeInstance
                    newUpgrade =
                    new UpgradeInstance(
                        foundData
                    );

                newUpgrade.currentLevel =
                    level;

                upgrades.Add(newUpgrade);
            }
        }

        RecalculateStats();

        Debug.Log(
            "Upgrades Loaded!"
        );
    }

    // =====================================
    // FIND UPGRADE DATA
    // =====================================

    UpgradeData FindUpgradeDataByName(
        string upgradeName
    )
    {
        foreach (
            var data in allUpgradeData
        )
        {
            if (
                data.upgradeName ==
                upgradeName
            )
            {
                return data;
            }
        }

        return null;
    }

    // =====================================
    // RESET UPGRADES
    // =====================================

    public void ResetUpgrades()
    {
        upgrades.Clear();

        PlayerPrefs.DeleteKey(
            "UPGRADES"
        );

        RecalculateStats();

        Debug.Log(
            "All upgrades reset."
        );
    }

    // =====================================
    // DEBUG
    // =====================================

    void DebugUpgrades()
    {
        Debug.Log(
            "===== UPGRADES ====="
        );

        foreach (var upgrade in upgrades)
        {
            Debug.Log(
                upgrade.data.upgradeName +
                " - " +
                upgrade.GetLevelText()
            );
        }
    }

    // =====================================
    // CHECK OWNED
    // =====================================

    public bool HasUpgrade(
        string upgradeName
    )
    {
        foreach (var upgrade in upgrades)
        {
            if (
                upgrade.data.upgradeName ==
                upgradeName
            )
            {
                return true;
            }
        }

        return false;
    }

    // =====================================
    // AHEAD OF SCHEDULE BONUS
    // =====================================

    public int AheadofSchedule()
    {
        if (aheadOfScheduleThreshhold < timerScript.GetRemainingTime())
        return currentAheadSchedBonus;
        else
        {
            return 0;
        }
    }

    // =====================================
    // CONSISTENCY BONUS
    // =====================================

    public float ConsistencyBonus()
    {
        float value = PlayerPrefs.GetInt("WinStreak");
        if (HasUpgrade("ConsistencyBonus") && value > 0)
        return value * currentconsistencymultiplier;
        else;
        return 1;
    }

public int GetUpgradeLevel(string upgradeName)
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.data.upgradeName == upgradeName)
            {
                return upgrade.currentLevel;
            }
        }

        return 0;
    }
   
}