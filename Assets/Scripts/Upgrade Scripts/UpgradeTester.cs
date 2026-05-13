using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class DebugTester : MonoBehaviour
{
    public UpgradeManager UpgradeManager;
    public CurrencySystem currencySystem;
    public TypingController RarityRoll;
    private UpgradeData testUpgrade;
    void Update()
    {
    }

    [ProButton]
    void AddUpgrade()
    {
        UpgradeManager.AddUpgrade(testUpgrade);
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
