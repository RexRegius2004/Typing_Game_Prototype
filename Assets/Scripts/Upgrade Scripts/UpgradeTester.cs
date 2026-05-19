using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class DebugTester : MonoBehaviour
{
    public UpgradeManager UpgradeManager;
    public CurrencySystem currencySystem;
    public TypingController RarityRoll;
    public UpgradeData[] testUpgrade;
    void Update()
    {
    }

    [ProButton]
    void AddWage()
    {
        UpgradeManager.AddUpgrade(testUpgrade[0]);
        Debug.Log("Upgrade Applied!");
    }

    [ProButton]
    void AddCritChance()
    {
        UpgradeManager.AddUpgrade(testUpgrade[1]);
        Debug.Log("Upgrade Applied!");
    }

    [ProButton]
    void AddCritHit()
    {
        UpgradeManager.AddUpgrade(testUpgrade[2]);
        Debug.Log("Upgrade Applied!");
    }

    [ProButton]
    void AddAheadSchedule()
    {
        UpgradeManager.AddUpgrade(testUpgrade[3]);
        Debug.Log("Upgrade Applied!");
    }

    [ProButton]
    void AddDelayTactics()
    {
        UpgradeManager.AddUpgrade(testUpgrade[4]);
        Debug.Log("Upgrade Applied!");
    }

    [ProButton]
    void AddConsistencyBonus()
    {
        UpgradeManager.AddUpgrade(testUpgrade[5]);
        Debug.Log("Upgrade Applied!");
    }

    [ProButton]
    void ResetUpgrades()
    {
        UpgradeManager.ResetUpgrades();
    }

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
}
