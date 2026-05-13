using UnityEngine;
using TMPro;
using System.Text;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject gameOverUI;
    public TextMeshProUGUI gameOverTextUI;
    public TextMeshProUGUI accuracyTextUI;
    public TextMeshProUGUI upgradesTextUI;
    public TextMeshProUGUI currencyTextUI;
    public TextMeshProUGUI P_RarityTextUI;

    public CurrencySystem currencySystem;
    public UpgradeManager UpgradeManager;
    public AccuracySystem accuracySystem;
    public TypingController P_rarity;

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
        UpdateRarityUI();
    }
    
    public void OpenGameOverUI(bool Outcome)
    {
        if (Outcome)
            gameOverTextUI.text = "You Win Nice Job!";
        else
            gameOverTextUI.text = "Game Over Time's Up";
        gameOverUI.SetActive(true);
    }

    public void UpdateUpgradeUI()
    {
        foreach (var upgrade in UpgradeManager.upgrades)
        {
            upgradesTextUI.text = $"Upgrades: \n {upgrade.data.upgradeName} Lv.{upgrade.currentLevel}/{upgrade.data.maxLevel}\n";
        }

    }
    public void AccuracyResultUI(float accuracy)
    {

        if (accuracy < accuracySystem.passThreshold)
        {
            accuracyTextUI.text = "No Rewards";
            Debug.Log("Failed - No rewards");
        }
        else
        {
            accuracyTextUI.text = "Give Rewards";
            Debug.Log("Passed - Give rewards");
        }

         accuracyTextUI.text = $"Accuracy: {accuracy:0}% \n {accuracyTextUI.text}";
    }

    public void UpdateCurrencyUI()
    {
        currencyTextUI.text = $"${currencySystem.Money.ToString()}";
    }

    public void UpdateRarityUI()
    {
        if (P_RarityTextUI != null)
        {
            P_RarityTextUI.text = $"Rarity: {P_rarity.RarityTier}";
        }
    }


    
}
