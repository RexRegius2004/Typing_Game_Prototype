using UnityEngine;
using TMPro;
using System.Text;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject gameOverUI;
    public TextMeshProUGUI gameOverTextUI;
    public TextMeshProUGUI upgradesTextUI;
    public TextMeshProUGUI currencyTextUI;
    public TextMeshProUGUI breakdownTextUI;

    public CurrencySystem currencySystem;
    public UpgradeManager UpgradeManager;
    public RewardsSystem rewardsSystem;
  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUpgradeUI();
        UpdateCurrencyUI();
    }
    
    public void OpenGameOverUI(bool Outcome)
    {
        if (Outcome)
            gameOverTextUI.text = "You Win Nice Job!";
        else
            gameOverTextUI.text = "Game Over Time's Up";
        
        BreakdownResultsUI();
        gameOverUI.SetActive(true);
    }

    public void UpdateUpgradeUI()
    {
        foreach (var upgrade in UpgradeManager.upgrades)
        {
            upgradesTextUI.text = $"Upgrades: \n {upgrade.data.upgradeName} Lv.{upgrade.currentLevel}/{upgrade.data.maxLevel}\n";
        }

    }

    public void BreakdownResultsUI()
    {
        breakdownTextUI.text = $"REWARD BREAKDOWN \n Words Typed: {rewardsSystem.wordsTyped} \n Accuracy: {rewardsSystem.accuracy}% \n Remaining Time: {rewardsSystem.remainingTime} \n Difficulty Multiplier: {rewardsSystem.difficultyMultiplier} \n Reward: {rewardsSystem.finalMoney} ";
    }

    public void UpdateCurrencyUI()
    {
        currencyTextUI.text = $"${currencySystem.Money.ToString()}";
    }

    
}
