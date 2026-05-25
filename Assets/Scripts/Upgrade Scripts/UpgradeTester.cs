using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class DebugTester : MonoBehaviour
{
    [Header("References")]
    public UpgradeManager UpgradeManager;
    public CurrencySystem currencySystem;
    public TypingController RarityRoll;
    public MusicManager musicManager;

    [Header("Test Upgrades")]
    public UpgradeData[] testUpgrade;

    // =====================================
    // ADD UPGRADES
    // =====================================

    
    public void AddWage()
    {
        musicManager.PlayButtonClickSFX();
        UpgradeManager.AddUpgrade(testUpgrade[0]);
        UpgradeManager.RecalculateStats();
    }

   
    public void AddCritChance()
    {
        musicManager.PlayButtonClickSFX();
        UpgradeManager.AddUpgrade(testUpgrade[1]);
        UpgradeManager.RecalculateStats();
    }


    public void AddCritHit()
    {
        musicManager.PlayButtonClickSFX();
        UpgradeManager.AddUpgrade(testUpgrade[2]);
        UpgradeManager.RecalculateStats();
    }


    public void AddAheadSchedule()
    {
        musicManager.PlayButtonClickSFX();
        UpgradeManager.AddUpgrade(testUpgrade[3]);
        UpgradeManager.RecalculateStats();
    }


    public void AddDelayTactics()
    {
        musicManager.PlayButtonClickSFX();
        UpgradeManager.AddUpgrade(testUpgrade[4]);
        UpgradeManager.RecalculateStats();
    }


    public void AddConsistencyBonus()
    {
        musicManager.PlayButtonClickSFX();
        UpgradeManager.AddUpgrade(testUpgrade[5]);
        UpgradeManager.RecalculateStats();
    }

    // =====================================
    // RESET UPGRADES
    // =====================================


    public void ResetUpgrades()
    {
        musicManager.PlayButtonClickSFX();
        UpgradeManager.ResetUpgrades();
    }

    // =====================================
    // SAVE / LOAD
    // =====================================

   
    public void SaveUpgrades()
    {
        musicManager.PlayButtonClickSFX();
        UpgradeManager.SaveUpgrades();

        Debug.Log("Manual Save Complete");
    }

  
    public void LoadUpgrades()
    {
        musicManager.PlayButtonClickSFX();
        UpgradeManager.LoadUpgrades();

        Debug.Log("Manual Load Complete");
    }

    // =====================================
    // DELETE SAVE
    // =====================================

   
    public void DeleteUpgradeSave()
    {
        musicManager.PlayButtonClickSFX();
        PlayerPrefs.DeleteKey("UPGRADES");

        Debug.Log("Upgrade Save Deleted");
    }

    // =====================================
    // DELETE ALL PLAYER PREFS
    // =====================================

    
    public void DeleteAllPlayerPrefs()
    {
        musicManager.PlayButtonClickSFX();
        PlayerPrefs.DeleteAll();

        Debug.Log("ALL PLAYER PREFS DELETED");
    }

    // =====================================
    // MONEY TESTING
    // =====================================

   
    public void AddMoney()
    {
        musicManager.PlayButtonClickSFX();
        currencySystem.AddMoney(50);
    }

   
    public void SubtractMoney()
    {
        musicManager.PlayButtonClickSFX();
        currencySystem.SubtractMoney(50);
    }

   
    public void ResetMoney()
    {
        musicManager.PlayButtonClickSFX();
        currencySystem.ResetMoney();
    }

    // =====================================
    // DEBUG
    // =====================================

   
    public void PrintUpgradeSave()
    {
        musicManager.PlayButtonClickSFX();
        Debug.Log(
            PlayerPrefs.GetString("UPGRADES")
        );
    }
}
