using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;
using DG.Tweening;


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
    public TextMeshProUGUI streakTextUI;
    public TextMeshProUGUI wpmTextUI;

    [Header("Streak Scale")]
    [SerializeField] float streakMinScale = 1f;
    [SerializeField] float streakMaxScale = 4f;
    [SerializeField] float streakScaleTweenDuration = 0.35f;
    [SerializeField] float streakPulseThreshold = 1.5f;
    [SerializeField] float streakPulseAmount = 0.1f;
    [SerializeField] float streakPulseDuration = 0.75f;

    float lastStreakDisplayed = -1f;


    

    public CurrencySystem currencySystem;
    public UpgradeManager UpgradeManager;
    public AccuracySystem accuracySystem;
    public TypingController P_rarity;
    public TextMeshProUGUI breakdownTextUI;
    public RewardsSystem rewardsSystem;
    public MusicManager musicManager;
    public GameObject longPromptChoiceUI;
    public TypingController typingController;

    [Header("Currency Change Feedback")]
    [SerializeField] float bumpStrength = 0.25f;
    [SerializeField] float bumpDuration = 0.35f;
    [SerializeField] int bumpVibrato = 6;
    [SerializeField] float bumpElasticity = 0.6f;
    [SerializeField] float rotateStrength = 12f;
    [SerializeField] float rotateDuration = 0.35f;
    [SerializeField] int rotateVibrato = 8;
    [SerializeField] float rotateElasticity = 0.5f;
    [SerializeField] float shakeDuration = 0.35f;
    [SerializeField] float shakeStrength = 6f;
    [SerializeField] int shakeVibrato = 18;
    [SerializeField] float shakeRandomness = 90f;

    int lastMoneyDisplayed;
    bool currencyUIReady;



    void Awake()
{
    // Script references — safe to auto-find by type
    if (rewardsSystem == null)
        rewardsSystem = FindAnyObjectByType<RewardsSystem>();
    if (typingController == null)
        typingController = FindAnyObjectByType<TypingController>();
    if (musicManager == null)
        musicManager = FindAnyObjectByType<MusicManager>();
    if (currencySystem == null)
        currencySystem = FindAnyObjectByType<CurrencySystem>();
    if (UpgradeManager == null)
        UpgradeManager = FindAnyObjectByType<UpgradeManager>();

    if (gameOverUI == null) Debug.LogError("[UIManager] gameOverUI not assigned!");
    if (UpgradePanel == null) Debug.LogError("[UIManager] UpgradePanel not assigned!");
    if (breakdownTextUI == null) Debug.LogError("[UIManager] breakdownTextUI not assigned!");

}
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
        UpdateStreakUI();
        UpdateWpmUI();
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
        int money = currencySystem.Money;
        currencyTextUI.text = $"${money}";
        currencyTextUI2.text = $"${money}"; //PlaceHolder For now, cant solve sorting layer issue.

        if (!currencyUIReady)
        {
            lastMoneyDisplayed = money;
            currencyUIReady = true;
            return;
        }

        if (money != lastMoneyDisplayed)
        {
            lastMoneyDisplayed = money;
            PlayCurrencyChangeFeedback();
        }
    }

    void PlayCurrencyChangeFeedback()
    {
        AnimateCurrencyText(currencyTextUI);
        AnimateCurrencyText(currencyTextUI2);
    }

    void AnimateCurrencyText(TextMeshProUGUI tmp)
    {
        if (tmp == null) return;

        RectTransform rt = tmp.rectTransform;
        rt.DOKill();
        rt.localScale = Vector3.one;
        rt.localRotation = Quaternion.identity;

        rt.DOPunchScale(Vector3.one * bumpStrength, bumpDuration, bumpVibrato, bumpElasticity);
        rt.DOPunchRotation(new Vector3(0f, 0f, rotateStrength), rotateDuration, rotateVibrato, rotateElasticity);
        rt.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness, false, true);
    }

    public void UpdateStreakUI()
    {
        if (streakTextUI == null || rewardsSystem == null) return;

        float streak = rewardsSystem.streakMultiplier;
        streakTextUI.text = $"{streak:0.00}x";

        if (Mathf.Approximately(streak, lastStreakDisplayed))
            return;

        lastStreakDisplayed = streak;

        float targetScale = Mathf.Clamp(streak, streakMinScale, streakMaxScale);
        RectTransform rt = streakTextUI.rectTransform;
        rt.DOKill(false);

        Tween scaleTween = rt
            .DOScale(Vector3.one * targetScale, streakScaleTweenDuration)
            .SetEase(Ease.OutBack);

        if (streak >= streakPulseThreshold)
        {
            scaleTween.OnComplete(() => StartStreakPulse(rt, targetScale));
        }
    }

    void StartStreakPulse(RectTransform rt, float baseScale)
    {
        if (rt == null) return;

        float inwardScale = baseScale * (1f - streakPulseAmount);
        rt.DOScale(Vector3.one * inwardScale, streakPulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void UpdateWpmUI()
    {
        if (wpmTextUI == null || typingController == null) return;
        wpmTextUI.text = $"{Mathf.RoundToInt(typingController.currentWPM)} WPM";
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
           // musicManager.PlayButtonClickSFX();
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
            //Phase2.SetActive(true);
        }
    }

public void ClosePhase2()
    {
        Phase2.SetActive(false);
        SceneManager.LoadScene("Phase2");
    }

//This is only temporary as Phase 2 is being developed 
public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
