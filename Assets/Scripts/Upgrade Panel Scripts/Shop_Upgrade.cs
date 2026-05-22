using UnityEngine;

public class Shop_Upgrade : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public CurrencySystem currencySystem;
    public UpgradeData[] upgradeData;
    public int[] UpgradeCosts;

    public void Awake()
    {
        if (upgradeManager == null)
        {
            upgradeManager = FindAnyObjectByType<UpgradeManager>();
        }

        if (currencySystem == null)
        {
            currencySystem = FindAnyObjectByType<CurrencySystem>();
        }
    }

    public void BuyWage()
    {
        if (currencySystem.Money >= UpgradeCosts[0] && upgradeManager.GetUpgradeLevel("Wage") < 1)
        {
            currencySystem.Money -= UpgradeCosts[0];
            upgradeManager.AddUpgrade(upgradeData[0]);
            Debug.Log(UpgradeCosts[0]); //Base Cost 100
        }
         else if (currencySystem.Money >= UpgradeCosts[0] * upgradeManager.GetUpgradeLevel("Wage") * 2
          && upgradeManager.GetUpgradeLevel("Wage") >= 1)
          // If player has enough or equal money compared to the Upgrade Cost and the Level is more than 1
          // then they can buy the upgrade, cost increases with each level
        {
            // For levels above 1, cost doubles
             upgradeManager.AddUpgrade(upgradeData[0]);
             currencySystem.Money -= (UpgradeCosts[0] * upgradeManager.GetUpgradeLevel("Wage")) * 2; // Increase cost for next level
             //Base cost * current level * 2 (for doubling) = new cost
             // 100 * 2 (for level 2) * 2 (for doubling) = 400 
             // 100 * 3 (for level 3) * 2 (for doubling) = 600 
             // 100 * 4 (for level 4) * 2 (for doubling) = 800 
             // 100 * 5 (for level 5) * 2 (for doubling) = 1000
             Debug.Log(UpgradeCosts[0] * upgradeManager.GetUpgradeLevel("Wage") * 2);
        }
         else if (upgradeManager.GetUpgradeLevel("Wage") == 5)
        {
            //Nothing will happen, max level reached
        }

       
    }

    public void BuyCritChance()
    {
       
        if (currencySystem.Money >= UpgradeCosts[1] && upgradeManager.GetUpgradeLevel("Critical Chance") < 1)
        {
            currencySystem.Money -= UpgradeCosts[1];
            upgradeManager.AddUpgrade(upgradeData[1]);
            Debug.Log(UpgradeCosts[1]); //Base Cost 100
        }
         else if (currencySystem.Money >= UpgradeCosts[1] * upgradeManager.GetUpgradeLevel("Critical Chance") * 2
          && upgradeManager.GetUpgradeLevel("Critical Chance") >= 1)
          // If player has enough or equal money compared to the Upgrade Cost and the Level is more than 1
          // then they can buy the upgrade, cost increases with each level
        {
            // For levels above 1, cost doubles
             upgradeManager.AddUpgrade(upgradeData[1]);
             currencySystem.Money -= (UpgradeCosts[1] * upgradeManager.GetUpgradeLevel("Critical Chance")) * 2; // Increase cost for next level
             //Base cost * current level * 2 (for doubling) = new cost
             // 100 * 2 (for level 2) * 2 (for doubling) = 400 
             // 100 * 3 (for level 3) * 2 (for doubling) = 600 
             // 100 * 4 (for level 4) * 2 (for doubling) = 800 
             // 100 * 5 (for level 5) * 2 (for doubling) = 1000
             Debug.Log(UpgradeCosts[1] * upgradeManager.GetUpgradeLevel("Critical Chance") * 2);
        }
         else if (upgradeManager.GetUpgradeLevel("Critical Chance") == 5)
        {
            //Nothing will happen, max level reached
        }


    }   

    public void BuyCritHit()
    {
        if (currencySystem.Money >= UpgradeCosts[2] && upgradeManager.GetUpgradeLevel("Critical Hit") < 1)
        {
            currencySystem.Money -= UpgradeCosts[2];
            upgradeManager.AddUpgrade(upgradeData[2]);
            Debug.Log(UpgradeCosts[2]); //Base Cost 100
        }
         else if (currencySystem.Money >= UpgradeCosts[2] * upgradeManager.GetUpgradeLevel("Critical Hit") * 2
          && upgradeManager.GetUpgradeLevel("Critical Hit") >= 1)
          // If player has enough or equal money compared to the Upgrade Cost and the Level is more than 1
          // then they can buy the upgrade, cost increases with each level
        {
            // For levels above 1, cost doubles
             upgradeManager.AddUpgrade(upgradeData[2]);
             currencySystem.Money -= (UpgradeCosts[2] * upgradeManager.GetUpgradeLevel("Critical Hit")) * 2; // Increase cost for next level
             //Base cost * current level * 2 (for doubling) = new cost
             // 100 * 2 (for level 2) * 2 (for doubling) = 400 
             // 100 * 3 (for level 3) * 2 (for doubling) = 600 
             // 100 * 4 (for level 4) * 2 (for doubling) = 800 
             // 100 * 5 (for level 5) * 2 (for doubling) = 1000
             Debug.Log(UpgradeCosts[2] * upgradeManager.GetUpgradeLevel("Critical Hit") * 2);
        }
         else if (upgradeManager.GetUpgradeLevel("Critical Hit") == 5)
        {
            //Nothing will happen, max level reached
        }
    }

    public void BuyAheadSchedule()
    {
       if (currencySystem.Money >= UpgradeCosts[3] && upgradeManager.GetUpgradeLevel("Ahead Schedule") < 1)
        {
            currencySystem.Money -= UpgradeCosts[3];
            upgradeManager.AddUpgrade(upgradeData[3]);
            Debug.Log(UpgradeCosts[3]); //Base Cost 100
        }
         else if (currencySystem.Money >= UpgradeCosts[3] * upgradeManager.GetUpgradeLevel("Ahead Schedule") * 2
          && upgradeManager.GetUpgradeLevel("Ahead Schedule") >= 1)
          // If player has enough or equal money compared to the Upgrade Cost and the Level is more than 1
          // then they can buy the upgrade, cost increases with each level
        {
            // For levels above 1, cost doubles
             upgradeManager.AddUpgrade(upgradeData[3]);
             currencySystem.Money -= (UpgradeCosts[3] * upgradeManager.GetUpgradeLevel("Ahead Schedule")) * 2; // Increase cost for next level
             //Base cost * current level * 2 (for doubling) = new cost
             // 100 * 2 (for level 2) * 2 (for doubling) = 400 
             // 100 * 3 (for level 3) * 2 (for doubling) = 600 
             // 100 * 4 (for level 4) * 2 (for doubling) = 800 
             // 100 * 5 (for level 5) * 2 (for doubling) = 1000
             Debug.Log(UpgradeCosts[3] * upgradeManager.GetUpgradeLevel("Ahead Schedule") * 2);
        }
         else if (upgradeManager.GetUpgradeLevel("Ahead Schedule") == 5)
        {
            //Nothing will happen, max level reached
        }
    }

    public void BuyDelayTactics()
    {
        if (currencySystem.Money >= UpgradeCosts[4] && upgradeManager.GetUpgradeLevel("Delay Tactics") < 1)
        {
            currencySystem.Money -= UpgradeCosts[4];
            upgradeManager.AddUpgrade(upgradeData[4]);
            Debug.Log(UpgradeCosts[4]); //Base Cost 100
        }
         else if (currencySystem.Money >= UpgradeCosts[4] * upgradeManager.GetUpgradeLevel("Delay Tactics") * 2
          && upgradeManager.GetUpgradeLevel("Delay Tactics") >= 1)
          // If player has enough or equal money compared to the Upgrade Cost and the Level is more than 1
          // then they can buy the upgrade, cost increases with each level
        {
            // For levels above 1, cost doubles
             upgradeManager.AddUpgrade(upgradeData[4]);
             currencySystem.Money -= (UpgradeCosts[4] * upgradeManager.GetUpgradeLevel("Delay Tactics")) * 2; // Increase cost for next level
             //Base cost * current level * 2 (for doubling) = new cost
             // 100 * 2 (for level 2) * 2 (for doubling) = 400 
             // 100 * 3 (for level 3) * 2 (for doubling) = 600 
             // 100 * 4 (for level 4) * 2 (for doubling) = 800 
             // 100 * 5 (for level 5) * 2 (for doubling) = 1000
             Debug.Log(UpgradeCosts[4] * upgradeManager.GetUpgradeLevel("Delay Tactics") * 2);
        }
         else if (upgradeManager.GetUpgradeLevel("Delay Tactics") == 5)
        {
            //Nothing will happen, max level reached
        }
    }

    public void BuyConsistencyBonus()
    {
       if (currencySystem.Money >= UpgradeCosts[5] && upgradeManager.GetUpgradeLevel("Consistency Bonus") < 1)
        {
            currencySystem.Money -= UpgradeCosts[5];
            upgradeManager.AddUpgrade(upgradeData[5]);
            Debug.Log(UpgradeCosts[5]); //Base Cost 100
        }
         else if (currencySystem.Money >= UpgradeCosts[5] * upgradeManager.GetUpgradeLevel("Consistency Bonus") * 2
          && upgradeManager.GetUpgradeLevel("Consistency Bonus") >= 1)
          // If player has enough or equal money compared to the Upgrade Cost and the Level is more than 1
          // then they can buy the upgrade, cost increases with each level
        {
            // For levels above 1, cost doubles
             upgradeManager.AddUpgrade(upgradeData[5]);
             currencySystem.Money -= (UpgradeCosts[5] * upgradeManager.GetUpgradeLevel("Consistency Bonus")) * 2; // Increase cost for next level
             //Base cost * current level * 2 (for doubling) = new cost
             // 100 * 2 (for level 2) * 2 (for doubling) = 400 
             // 100 * 3 (for level 3) * 2 (for doubling) = 600 
             // 100 * 4 (for level 4) * 2 (for doubling) = 800 
             // 100 * 5 (for level 5) * 2 (for doubling) = 1000
             Debug.Log(UpgradeCosts[5] * upgradeManager.GetUpgradeLevel("Consistency Bonus") * 2);
        }
         else if (upgradeManager.GetUpgradeLevel("Consistency Bonus") == 5)
        {
            //Nothing will happen, max level reached
        }
    }

}
