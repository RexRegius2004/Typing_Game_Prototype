using System.Data.Common;
using UnityEngine;
using TMPro;

public class Shop_Upgrade : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public CurrencySystem currencySystem;
    public UpgradeData[] upgradeData;
    public MusicManager musicManager;
    public int[] UpgradeCosts;

   public TextMeshProUGUI wageCostText;
    public TextMeshProUGUI critChanceCostText;
    public TextMeshProUGUI critHitCostText;
    public TextMeshProUGUI aheadScheduleCostText;
    public TextMeshProUGUI delayTacticsCostText;
    public TextMeshProUGUI consistencyBonusCostText;

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

void Start()
    {
        wageCostText.text = GetCost("Wage", 0).ToString() + " $";
        critChanceCostText.text = GetCost("Critical Chance", 1).ToString() + " $";
        critHitCostText.text = GetCost("Critical Hit", 2).ToString() + " $";
        aheadScheduleCostText.text = GetCost("Ahead of Schedule", 3).ToString() + " $";
        delayTacticsCostText.text = GetCost("Delay Tactics", 4).ToString() + " $";
        consistencyBonusCostText.text = GetCost("Consistency Bonus", 5).ToString() + " $";
    }
void Update()
        {
           
        }
    
    public void BuyWage()
    {
        int currentLevel = upgradeManager.GetUpgradeLevel("Wage");
        int cost = currentLevel == 0 
            ? UpgradeCosts[0] 
            : UpgradeCosts[0] * (currentLevel + 1) * 2;
        if (currencySystem.Money < cost)
        {
            return; // Not enough money
            //Open Panel to inform player they don't have enough money
        }

        musicManager.PlayButtonClickSFX();
        currencySystem.Money -= cost;
        upgradeManager.AddUpgrade(upgradeData[0]);
        RefreshUpgradeCosts();
        Debug.Log(cost);
    }

    public void BuyCritChance()
    {
        int currentLevel = upgradeManager.GetUpgradeLevel("Critical Chance");
        int cost = currentLevel == 0 
            ? UpgradeCosts[1] 
            : UpgradeCosts[1] * (currentLevel + 1) * 2;
        
        if (currencySystem.Money < cost)
        {
            return; // Not enough money
        }

        musicManager.PlayButtonClickSFX();
        currencySystem.Money -= cost;
        upgradeManager.AddUpgrade(upgradeData[1]);
        RefreshUpgradeCosts();
        Debug.Log(cost);

    }   

    public void BuyCritHit()
    {
        int currentLevel = upgradeManager.GetUpgradeLevel("Critical Hit");
        int cost = currentLevel == 0 
            ? UpgradeCosts[2] 
            : UpgradeCosts[2] * (currentLevel + 1) * 2;
        ;
        if (currencySystem.Money < cost)      
          {
            return; // Not enough money
          }

        musicManager.PlayButtonClickSFX();
        currencySystem.Money -= cost;
        upgradeManager.AddUpgrade(upgradeData[2]);
        RefreshUpgradeCosts();
        Debug.Log(cost);
       
    }

    public void BuyAheadSchedule()
    {
        int currentLevel = upgradeManager.GetUpgradeLevel("Ahead of Schedule");
        int cost = currentLevel == 0
            ? UpgradeCosts[3]
            : UpgradeCosts[3] * (currentLevel + 1) * 2;
        
        if (currencySystem.Money < cost)
        {
            return; // Not enough money
        }

        musicManager.PlayButtonClickSFX();
        currencySystem.Money -= cost;
        upgradeManager.AddUpgrade(upgradeData[3]);
        RefreshUpgradeCosts();
        Debug.Log(cost);





      
    }

    public void BuyDelayTactics()
    {
        int currentLevel = upgradeManager.GetUpgradeLevel("Delay Tactics");
        int cost = currentLevel == 0
            ? UpgradeCosts[4]
            : UpgradeCosts[4] * (currentLevel + 1) * 2;
        ;
        if (currencySystem.Money < cost)
        {
            return; // Not enough money
        }

        musicManager.PlayButtonClickSFX();
        currencySystem.Money -= cost;
        upgradeManager.AddUpgrade(upgradeData[4]);
        RefreshUpgradeCosts();
        Debug.Log(cost);
    }

    public void BuyConsistencyBonus()
    {
        int currentLevel = upgradeManager.GetUpgradeLevel("Consistency Bonus");
        int cost = currentLevel == 0
            ? UpgradeCosts[5]
            : UpgradeCosts[5] * (currentLevel + 1) * 2;
               if (currencySystem.Money < cost)
        {
            return; // Not enough money
        }

        musicManager.PlayButtonClickSFX();
        currencySystem.Money -= cost;
        upgradeManager.AddUpgrade(upgradeData[5]);
        RefreshUpgradeCosts();
        Debug.Log(cost);
    }
    void RefreshUpgradeCosts()
    {
        wageCostText.text = GetCost("Wage", 0).ToString() + " $";
        critChanceCostText.text = GetCost("Critical Chance", 1).ToString() + " $";
        critHitCostText.text = GetCost("Critical Hit", 2).ToString() + " $";
        aheadScheduleCostText.text = GetCost("Ahead of Schedule", 3).ToString() + " $";
        delayTacticsCostText.text = GetCost("Delay Tactics", 4).ToString() + " $";
        consistencyBonusCostText.text = GetCost("Consistency Bonus", 5).ToString() + " $";
    }

    int GetCost(string upgradeName, int costIndex)
{
    int currentLevel = upgradeManager.GetUpgradeLevel(upgradeName);
    return currentLevel == 0
        ? UpgradeCosts[costIndex]
        : UpgradeCosts[costIndex] * (currentLevel + 1) * 2;
}
}
