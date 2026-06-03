using UnityEngine;
using TMPro;
using System.Text;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject gameOverUI;
    public GameObject UpgradePanel;
    public GameObject DebugPanel;
    public GameObject P_RarityText;
    public GameObject Phase2;
    public TextMeshProUGUI gameOverTextUI;
    public TextMeshProUGUI upgradesTextUI;
    public TextMeshProUGUI currencyTextUI;
    public TextMeshProUGUI currencyTextUI2;
    public TextMeshProUGUI P_RarityTextUI;


    

    public CurrencySystem currencySystem;
    public UpgradeManager UpgradeManager;
    public AccuracySystem accuracySystem;
    public TypingController P_rarity;
    public TextMeshProUGUI breakdownTextUI;
    public RewardsSystem rewardsSystem;
    public MusicManager musicManager;
    public GameObject longPromptChoiceUI;
    public TypingController typingController;
  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverUI.SetActive(false);
        UpgradePanel.SetActive(false);
        DebugPanel.SetActive(false);
        P_RarityText.SetActive(false);
        Phase2.SetActive(false);
         // Hide rarity UI at start, will show when a prompt is active
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUpgradeUI();
        UpdateCurrencyUI();
        UpdateRarityUI();
        PlayerPromotion();
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
        if (UpgradeManager == null || UpgradeManager.upgrades == null) return;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Upgrades:");

        foreach (var upgrade in UpgradeManager.upgrades)
        {
            sb.AppendLine($"{upgrade.data.upgradeName} Lv.{upgrade.currentLevel}\n");
        }

        upgradesTextUI.text = sb.ToString();

    }

    public void BreakdownResultsUI()
{
    breakdownTextUI.text =
        $"REWARD BREAKDOWN\n" +
        $"Words Typed: {rewardsSystem.lastWordsTyped}\n" +
        $"Accuracy: {rewardsSystem.lastAccuracy}%\n" +
        $"Remaining Time: {rewardsSystem.lastRemainingTime}\n" +
        $"Difficulty Multiplier: {rewardsSystem.lastDifficultyMultiplier}\n" +
        $"Reward: {rewardsSystem.finalMoney}\n" +
        $"CritMoney: {rewardsSystem.criticalMoney}";
}

    public void UpdateCurrencyUI()
    {
        currencyTextUI.text = $"${currencySystem.Money.ToString()}";
        currencyTextUI2.text = $"${currencySystem.Money.ToString()}"; //PlaceHolder For now, cant solve sorting layer issue.
    }

    public void UpdateRarityUI()
    {
        if (P_RarityTextUI != null)
        {
            P_RarityTextUI.text = $"Rarity: {P_rarity.currentPromptRarity}";
        }
    }

    public void OpenUpgradePanel()
    {
        if (UpgradePanel != null)
        {
            musicManager.PlayButtonClickSFX();
            UpgradePanel.SetActive(!UpgradePanel.activeSelf); 
            Time.timeScale = UpgradePanel.activeSelf ? 0 : 1; 
            // Pause game when panel is open and resume when closed
        }

    }

    public void OpenDebugPanel()
    {
        if (DebugPanel != null)
        {
            musicManager.PlayButtonClickSFX();
            DebugPanel.SetActive(!DebugPanel.activeSelf);
            Time.timeScale = DebugPanel.activeSelf ? 0 : 1; 
             // Pause game when panel is open and resume when closed
        }
    }

 public void AcceptLongPrompt()
{
    typingController.pendingLongPrompt = false;

    if (longPromptChoiceUI != null)
        longPromptChoiceUI.SetActive(false);

    typingController.ResetLongPromptOfferTimer();
    typingController.StartLongPromptMode();
    P_RarityText.SetActive(true); 

    typingController.isGameActive = true;
}

public void DeclineLongPrompt()
{
    typingController.pendingLongPrompt = false;

    if (longPromptChoiceUI != null)
        longPromptChoiceUI.SetActive(false);

    typingController.ResetLongPromptOfferTimer();
    typingController.isGameActive = true;

    typingController.GenerateRandomWord();
}

public void PlayerPromotion()
    {
        if (currencySystem.Money >= 1000)
        {
            Phase2.SetActive(true);
        }
    }

public void ClosePhase2()
    {
        Phase2.SetActive(false);
    }
}
