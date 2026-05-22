using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class DebugTester : MonoBehaviour
{
    [Header("References")]
    public UpgradeManager UpgradeManager;
    public CurrencySystem currencySystem;
    public TypingController RarityRoll;

    [Header("Test Upgrades")]
    public UpgradeData[] testUpgrade;

    // =====================================
    // ADD UPGRADES
    // =====================================

    [ProButton]
    void AddWage()
    {
        UpgradeManager.AddUpgrade(testUpgrade[0]);
    }

    [ProButton]
    void AddCritChance()
    {
        UpgradeManager.AddUpgrade(testUpgrade[1]);
    }

    [ProButton]
    void AddCritHit()
    {
        UpgradeManager.AddUpgrade(testUpgrade[2]);
    }

    [ProButton]
    void AddAheadSchedule()
    {
        UpgradeManager.AddUpgrade(testUpgrade[3]);
    }

    [ProButton]
    void AddDelayTactics()
    {
        UpgradeManager.AddUpgrade(testUpgrade[4]);
    }

    [ProButton]
    void AddConsistencyBonus()
    {
        UpgradeManager.AddUpgrade(testUpgrade[5]);
    }

    // =====================================
    // RESET UPGRADES
    // =====================================

    [ProButton]
    void ResetUpgrades()
    {
        UpgradeManager.ResetUpgrades();
    }

    // =====================================
    // SAVE / LOAD
    // =====================================

    [ProButton]
    void SaveUpgrades()
    {
        UpgradeManager.SaveUpgrades();

        Debug.Log("Manual Save Complete");
    }

    [ProButton]
    void LoadUpgrades()
    {
        UpgradeManager.LoadUpgrades();

        Debug.Log("Manual Load Complete");
    }

    // =====================================
    // DELETE SAVE
    // =====================================

    [ProButton]
    void DeleteUpgradeSave()
    {
        PlayerPrefs.DeleteKey("UPGRADES");

        Debug.Log("Upgrade Save Deleted");
    }

    // =====================================
    // DELETE ALL PLAYER PREFS
    // =====================================

    [ProButton]
    void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

        Debug.Log("ALL PLAYER PREFS DELETED");
    }

    // =====================================
    // MONEY TESTING
    // =====================================

    [ProButton]
    void AddMoney()
    {
        currencySystem.AddMoney(50);
    }

    [ProButton]
    void SubtractMoney()
    {
        currencySystem.SubtractMoney(50);
    }

    [ProButton]
    void ResetMoney()
    {
        currencySystem.ResetMoney();
    }

    // =====================================
    // DEBUG
    // =====================================

    [ProButton]
    void PrintUpgradeSave()
    {
        Debug.Log(
            PlayerPrefs.GetString("UPGRADES")
        );
    }
}
