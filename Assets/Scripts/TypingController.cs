using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class TypingController : MonoBehaviour
{
    [Header("GAME MODE")]
    public TypingGameMode currentMode =
        TypingGameMode.QuickWords;

[Header("Contract Choice UI")]
public GameObject longPromptChoiceUI;
public bool pendingLongPrompt = false;

    // =====================================
    // QUICK WORD MODE
    // =====================================

    [Header("Quick Words By Length")]
    [Tooltip("Lengths unlocked at the start of the game")]
    [SerializeField] List<int> startingUnlockedLengths = new List<int>() { 3, 4, 5 };

    public List<string> words3 = new List<string>() { "nda", "pay", "tax", "ceo", "hr" };
    public List<string> words4 = new List<string>() { "memo", "deal", "file", "task", "loan" };
    public List<string> words5 = new List<string>() { "email", "sales", "audit", "draft", "inbox" };
    public List<string> words6 = new List<string>() { "report", "urgent", "client", "budget", "office" };
    public List<string> words7 = new List<string>() { "meeting", "invoice", "manager", "project", "payroll" };
    public List<string> words8 = new List<string>() { "deadline", "schedule", "feedback", "proposal", "strategy" };
    public List<string> words9 = new List<string>() { "signature", "marketing", "inventory", "workspace" };
    public List<string> words10 = new List<string>() { "attachment", "accounting", "management", "department" };

    [Tooltip("Seconds between long prompt offers in quick mode")]
    public float longPromptOfferInterval = 10f;

    private float longPromptOfferTimer = 0f;
    private readonly HashSet<int> unlockedLengths = new HashSet<int>();
    private readonly List<string> mainWordPool = new List<string>();

    // =====================================
    // CURRENT TEXT
    // =====================================

    [Header("Current Text")]
    public string targetText;

    private string typedText = "";
    private int currentIndex = 0;
    private bool[] critLetters;

    // =====================================
    // UI
    // =====================================

    [Header("UI")]
    public TextMeshProUGUI targetTextUI;

    [Header("Target Text Bounce")]
    [SerializeField] float targetBumpStrength = 0.25f;
    [SerializeField] float targetBumpDuration = 0.35f;
    [SerializeField] int targetBumpVibrato = 6;
    [SerializeField] float targetBumpElasticity = 0.6f;

    string lastTargetDisplayed;

    // =====================================
    // REFERENCES
    // =====================================

    [Header("Systems")]
    public RewardsSystem rewardsSystem;
    public AccuracySystem accuracySystem;
    public CurrencySystem currencySystem;
    public UpgradeManager upgradeManager;
    public TimerScript timerScript;
    public UIManager uiManager;
    public MusicManager musicManager;
    public WordAssembler wordAssembler;

    // =====================================
    // PROMPT RARITY
    // =====================================

    [Header("Long Prompt System")]
    public Prompt_Rarity Prompt_Tier;

    [HideInInspector]
    public string currentPromptRarity =
        "Common";

    // =====================================
    // GAME STATE
    // =====================================

    public bool isGameActive = true;

    // =====================================
    // START
    // =====================================

    // =====================================
    // INCORRECT SHAKE
    // =====================================
    [Header("Incorrect Shake")]
    [SerializeField] float mistakeShakeDuration = 0.4f;
    [SerializeField] float mistakeShakeStrength = 14f;
    [SerializeField] int mistakeShakeVibrato = 22;
    [SerializeField] float mistakeShakeRandomness = 90f;
    [SerializeField] Color mistakeColor = Color.red;

    [Header("WPM")]
    public float currentWPM;
    int charsTyped;
    float typingElapsed;
    bool wpmTracking;
    bool wpmClockRunning;

    [Header("Word Timer")]
    [SerializeField] float wordTimeLimit = 5f;
    [SerializeField] TextMeshProUGUI wordTimerTextUI;
    float wordTimer;
    bool wordTimerActive;

    void Start()
    {
        if (musicManager == null)
        {
            musicManager = FindAnyObjectByType<MusicManager>();
        }

        
        Prompt_Tier =
            GameObject.Find("Prompt_Manager")
            .GetComponent<Prompt_Rarity>();

        InitUnlockedWordLengths();
        StartQuickWordMode();

        timerScript.OnTimerEnd +=
            HandleTimeUp;

        longPromptChoiceUI.SetActive(false);
        
    }

    // =====================================
    // UPDATE
    // =====================================

    void Update()
    {
        UpdateLongPromptOfferTimer();

        // Only count time while actively typing a word (not gaps / timeouts)
        if (wpmTracking && wpmClockRunning)
        {
            typingElapsed += Time.deltaTime;
            UpdateWPM();
        }

        if (!isGameActive)
            return;

        UpdateWordTimer();
        HandleTyping();
    }

    // =====================================
    // START QUICK MODE
    // =====================================

    public void StartQuickWordMode()
    {
        currentMode =
            TypingGameMode.QuickWords;

        ResetWPM();
        GenerateRandomWord();

        Debug.Log("QUICK MODE");
    }

    // =====================================
    // START LONG MODE
    // =====================================

    public void StartLongPromptMode()
    {
        currentMode =
            TypingGameMode.LongPrompt;

        StopWordTimer();

        targetText =
            Randomized_PromptRarity();
        timerScript.StartTimer();
        critLetters = new bool[targetText.Length];
        ResetTyping();

        UpdateTextUI();

        Debug.Log("LONG PROMPT MODE");
    }

    // =====================================
    // GENERATE QUICK WORD
    // =====================================

    public void GenerateRandomWord()
    {
        if (mainWordPool.Count == 0)
            RebuildMainWordPool();

        if (mainWordPool.Count == 0)
            return;

        int randomIndex = Random.Range(0, mainWordPool.Count);

        // Never repeat the previous word when another option exists
        if (mainWordPool.Count > 1 && !string.IsNullOrEmpty(targetText))
        {
            int attempts = 0;
            while (mainWordPool[randomIndex] == targetText && attempts < 10)
            {
                randomIndex = Random.Range(0, mainWordPool.Count);
                attempts++;
            }
        }

        targetText = mainWordPool[randomIndex];
        mainWordPool.RemoveAt(randomIndex);

        ResetTyping();
        PauseWpmClock();

        critLetters = new bool[targetText.Length];

        UpdateTextUI();
        StartWordTimer();
    }

    void InitUnlockedWordLengths()
    {
        unlockedLengths.Clear();

        if (startingUnlockedLengths == null || startingUnlockedLengths.Count == 0)
        {
            unlockedLengths.Add(3);
            unlockedLengths.Add(4);
            unlockedLengths.Add(5);
        }
        else
        {
            foreach (int length in startingUnlockedLengths)
            {
                if (length >= 3 && length <= 10)
                    unlockedLengths.Add(length);
            }
        }

        RebuildMainWordPool();
    }

    public bool UnlockWordLength(int length)
    {
        if (length < 3 || length > 10)
            return false;

        if (!unlockedLengths.Add(length))
            return false; // already unlocked

        List<string> words = GetWordListForLength(length);
        if (words != null && words.Count > 0)
            mainWordPool.AddRange(words);

        Debug.Log($"Unlocked {length}-letter words. Main pool size: {mainWordPool.Count}");
        return true;
    }

    public bool IsWordLengthUnlocked(int length)
    {
        return unlockedLengths.Contains(length);
    }

    void RebuildMainWordPool()
    {
        mainWordPool.Clear();

        foreach (int length in unlockedLengths)
        {
            List<string> words = GetWordListForLength(length);
            if (words == null || words.Count == 0)
                continue;

            mainWordPool.AddRange(words);
        }
    }

    List<string> GetWordListForLength(int length)
    {
        switch (length)
        {
            case 3: return words3;
            case 4: return words4;
            case 5: return words5;
            case 6: return words6;
            case 7: return words7;
            case 8: return words8;
            case 9: return words9;
            case 10: return words10;
            default: return null;
        }
    }

    void PlayIncorrectShakeThenReset()
    {
        if (targetTextUI == null) return;

        isGameActive = false;
        PauseWpmClock();

        RectTransform rt = targetTextUI.rectTransform;
        Color originalColor = targetTextUI.color;

        rt.DOKill();
        targetTextUI.DOKill();

        Sequence seq = DOTween.Sequence();
        seq.SetTarget(rt);
        seq.Append(
            rt.DOShakeAnchorPos(
                mistakeShakeDuration,
                new Vector2(mistakeShakeStrength, 0f),
                mistakeShakeVibrato,
                mistakeShakeRandomness,
                false,
                true
            )
        );
        seq.Join(targetTextUI.DOColor(mistakeColor, mistakeShakeDuration));
        seq.OnComplete(() =>
        {
            targetTextUI.color = originalColor;
            rewardsSystem.ResetStreak();
            GenerateRandomWord();
            wordAssembler.Erase();
            isGameActive = true;
        });
    }

    bool IsCritLetter(int index)
    {
        return critLetters != null &&
            index >= 0 &&
            index < critLetters.Length &&
            critLetters[index];
    }

    void UpdateLongPromptOfferTimer()
    {
        if (currentMode != TypingGameMode.QuickWords)
            return;

        if (pendingLongPrompt || !isGameActive)
            return;

        longPromptOfferTimer += Time.deltaTime;

        if (longPromptOfferTimer >= longPromptOfferInterval)
        {
            longPromptOfferTimer = 0f;
            ShowLongPromptOffer();
        }
    }

    public void ResetLongPromptOfferTimer()
    {
        longPromptOfferTimer = 0f;
    }

    void ShowLongPromptOffer()
    {
        if (pendingLongPrompt)
            return;

        pendingLongPrompt = true;
        isGameActive = false;

        if (longPromptChoiceUI != null)
            longPromptChoiceUI.SetActive(true);
    }

    // =====================================
    // RESET TYPING STATE
    // =====================================

    void ResetTyping()
    {
        typedText = "";
        currentIndex = 0;

        // IMPORTANT:
        // Reset accuracy per word
        accuracySystem.ResetAccuracy();
    }

    // =====================================
    // TYPE CHARACTER
    // =====================================

    void HandleTyping()
{
    foreach (char c in Input.inputString)
    {
        // BACKSPACE
        if (c == '\b')
        {
            HandleBackspace();
        }

        // IGNORE ENTER
        else if (
            c == '\n' ||
            c == '\r'
        )
        {
            continue;
        }

        // NORMAL INPUT
        else
        {
                Debug.Log(c);
                //Spawn Letter Here
            TypeCharacter(c);
        }
    }

    UpdateTextUI();

    // FINISH CURRENT TEXT
    if (
        typedText.Length >=
        targetText.Length
    )
    {
        CompleteCurrentText();
        wordAssembler.Erase();
    }
}

void TypeCharacter(char c)
{
    if (
        currentIndex >=
        targetText.Length
    )
        return;

    char expectedChar =
        targetText[currentIndex];

    if (c != expectedChar && currentMode == TypingGameMode.QuickWords)
    {
        // musicManager.RepeatGameSFX();
        RegisterTypedChar();
        PlayIncorrectShakeThenReset();
        return;
    }

    RegisterTypedChar();

    if (c == expectedChar)
    {
        critLetters[currentIndex] =
            rewardsSystem.RollCritLetter();

        if (critLetters[currentIndex])
        {
            wordAssembler.SpawnCriticalLetter(c);
            musicManager.PlayCriticalHitSFX();

            }
            else
            {
                wordAssembler.SpawnLetter(c);
            }
    }

    typedText += c;

    accuracySystem.RegisterInput(
        c,
        expectedChar
    );

    currentIndex++;

    if (c == expectedChar)
    {
            // musicManager.PlayCorrectKeySFX();

    }
    else
    {
        musicManager.PlayIncorrectKeySFX();
        
    }
}
    // =====================================
    // BACKSPACE
    // =====================================

    void HandleBackspace()
    {
        if (typedText.Length <= 0)
            return;

        typedText =
            typedText.Substring(
                0,
                typedText.Length - 1
            );

        currentIndex--;
    }

    // =====================================
    // COMPLETE
    // =====================================

    void CompleteCurrentText()
    {
        accuracySystem.CalculateFinalAccuracy();
       // musicManager.FinishedWord();

        // ---------------------------------
        // QUICK WORD MODE
        // ---------------------------------

         if (currentMode == TypingGameMode.QuickWords)
         
        {
            if (typedText == targetText)
        {
            int reward =
                rewardsSystem
                .CalculateQuickReward(
                    targetText,
                    typedText,
                    accuracySystem.finalAccuracy,
                    critLetters
                );

            rewardsSystem.AddStreak();

            Debug.Log(
                "WORD COMPLETE +" + reward);
        }
        else
            {
                rewardsSystem.ResetStreak();
                Debug.Log("MISTYPED WORD: No Reward");
            }

            GenerateRandomWord();
        }

        // ---------------------------------
        // LONG PROMPT MODE
        // ---------------------------------

        else
        {
            timerScript.StopTimer();

            rewardsSystem
            .CalculateLongPromptReward(
                this,
                accuracySystem,
                timerScript
            );

            uiManager.OpenGameOverUI(true);

            Debug.Log(
                "LONG PROMPT COMPLETE"
            );

            isGameActive = false;
        }
    }

    // =====================================
    // UPDATE UI
    // =====================================

    void UpdateTextUI()
    {
        string result = "";

        for (
            int i = 0;
            i < targetText.Length;
            i++
        )
        {
            char targetChar =
                targetText[i];

            result += targetChar;
        }

        targetTextUI.text = result;

        if (targetText != lastTargetDisplayed)
        {
            lastTargetDisplayed = targetText;
            BounceTargetText();
        }
    }

    void BounceTargetText()
    {
        if (targetTextUI == null) return;

        RectTransform rt = targetTextUI.rectTransform;
        rt.DOKill(false);
        rt.localScale = Vector3.one;
        rt.DOPunchScale(Vector3.one * targetBumpStrength, targetBumpDuration, targetBumpVibrato, targetBumpElasticity);
    }

    void ResetWPM()
    {
        charsTyped = 0;
        typingElapsed = 0f;
        currentWPM = 0f;
        wpmTracking = false;
        wpmClockRunning = false;
    }

    void PauseWpmClock()
    {
        wpmClockRunning = false;
    }

    void RegisterTypedChar()
    {
        wpmTracking = true;
        wpmClockRunning = true;
        charsTyped++;
        UpdateWPM();
    }

    void UpdateWPM()
    {
        if (typingElapsed <= 0.01f)
        {
            currentWPM = 0f;
            return;
        }

        // Typical typing-test WPM: 5 letters = 1 word
        float minutes = typingElapsed / 60f;
        currentWPM = (charsTyped / 5f) / minutes;
    }

    void StartWordTimer()
    {
        if (currentMode != TypingGameMode.QuickWords)
        {
            wordTimerActive = false;
            return;
        }

        wordTimer = wordTimeLimit;
        wordTimerActive = true;
        UpdateWordTimerUI();
    }

    void StopWordTimer()
    {
        wordTimerActive = false;
    }

    void UpdateWordTimer()
    {
        if (!wordTimerActive || currentMode != TypingGameMode.QuickWords)
            return;

        wordTimer -= Time.deltaTime;
        UpdateWordTimerUI();

        if (wordTimer > 0f)
            return;

        wordTimer = 0f;
        wordTimerActive = false;
        OnWordTimerExpired();
    }

    void OnWordTimerExpired()
    {
        PauseWpmClock();
        rewardsSystem.ResetStreak();
        wordAssembler.Erase();
        GenerateRandomWord();
    }

    void UpdateWordTimerUI()
    {
        if (wordTimerTextUI == null) return;
        wordTimerTextUI.text = $"{Mathf.CeilToInt(Mathf.Max(0f, wordTimer))}";
    }

    void OldUpdateTextUI()
    {
        string result = "";

        for (
            int i = 0;
            i < targetText.Length;
            i++
        )
        {
            char targetChar =
                targetText[i];

            bool isCritLetter =
                i < typedText.Length &&
                IsCritLetter(i);

            // CORRECT
            if (i < typedText.Length)
            {
                if (
                    typedText[i] ==
                    targetChar
                )
                {
                    result += isCritLetter
                        ? $"<color=yellow>{targetChar}</color>"
                        : $"<color=white>{targetChar}</color>";

                }
                else
                {
                    result +=
                        $"<color=red>{targetChar}</color>";
                }
            }

            // CURRENT
            else if (i == currentIndex)
            {
                result += isCritLetter
                    ? $"<mark=#FFFF0044><color=yellow>{targetChar}</color></mark>"
                    : $"<mark=#FFFFFF44>{targetChar}</mark>";
            }

            // UNTOUCHED
            else
            {
                result += isCritLetter
                    ? $"<color=yellow>{targetChar}</color>"
                    : $"<color=#1a1a2e>{targetChar}</color>";
            }
        }

        targetTextUI.text = result;
    }

    // =====================================
    // PROMPT RARITY
    // =====================================

    public string Randomized_PromptRarity()
{
    var allRows =
        Prompt_Tier.promptList.PromptRarity;

    int randomIndex =
        Random.Range(
            0,
            allRows.Length
        );

    var rows =
        allRows[randomIndex];

    float rarityRoll =
        Random.value;

    string chosenRarity =
        rows.Common;

    if (rarityRoll < 0.40f)
    {
        chosenRarity =
            rows.Common;

        currentPromptRarity =
            "Common";
    }
    else if (rarityRoll < 0.70f)
    {
        chosenRarity =
            rows.Uncommon;

        currentPromptRarity =
            "Uncommon";
    }
    else if (rarityRoll < 0.85f)
    {
        chosenRarity =
            rows.Rare;

        currentPromptRarity =
            "Rare";
    }
    else if (rarityRoll < 0.95f)
    {
        chosenRarity =
            rows.Epic;

        currentPromptRarity =
            "Epic";
    }
    else if (rarityRoll <= 1f)
    {
        chosenRarity =
            rows.Legendary;

        currentPromptRarity =
            "Legendary";
    }

    return chosenRarity;
}

    // =====================================
    // TIME UP
    // =====================================

    void HandleTimeUp()
    {
        if (
            currentMode !=
            TypingGameMode.LongPrompt
        )
            return;

        isGameActive = false;

        uiManager.OpenGameOverUI(false);

        Debug.Log("TIME UP");
    }

   
}


