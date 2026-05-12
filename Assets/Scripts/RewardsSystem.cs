using Unity.VisualScripting;
using UnityEngine;

public class RewardsSystem : MonoBehaviour
{
    /*[Header("Currency")]
    public CurrencySystem currencySystem;

    [Header("Upgrades")]
    public UpgradeManager upgradeManager;

    [Header("Rewards Results")]
    public int earnedMoney;
    public void GiveRewards(float accuracy)
    {
        earnedMoney =  Mathf.RoundToInt(accuracy * upgradeManager.currentWageMultiplier);

        if (accuracy >= 100f)
        {
            earnedMoney += upgradeManager.PerfectAccuracyBonus;

            Debug.Log("PERFECT ACCURACY BONUS!");
        }
        else if (accuracy >= 90f)
        {
            earnedMoney += upgradeManager.HighAccuracyBonus;

            Debug.Log("HIGH ACCURACY BONUS!");
        }
        
        foreach (var upgrade in upgradeManager.upgrades)
        {
            switch (upgrade.data.name)
            {
                case "Wage":
                earnedMoney *= upgradeManager.currentWageMultiplier/100;
                break;
            }
            //upgradesTextUI.text = $"Upgrades: \n {upgrade.data.upgradeName} Lv.{upgrade.currentLevel}/{upgrade.data.maxLevel}\n";
        }

        currencySystem.AddMoney(earnedMoney);
        earnedMoney = 0;
    } */
}
