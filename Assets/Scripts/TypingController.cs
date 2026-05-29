using UnityEngine;
using TMPro;
using System.Collections.Generic;

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

    [Header("Quick Words")]

    [Tooltip("Small words used in fast mode")]
    public List<string> quickWords =
        new List<string>()
        {
            "email",
            "meeting",
            "report",
            "invoice",
            "deadline",
            "urgent",
            "schedule",
            "manager",
            "client",
            "memo"
        };

    [Tooltip("Seconds between long prompt offers in quick mode")]
    public float longPromptOfferInterval = 10f;

    private float longPromptOfferTimer = 0f;

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

    void Start()
    {
        Prompt_Tier =
            GameObject.Find("Prompt_Random")
            .GetComponent<Prompt_Rarity>();

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

        if (!isGameActive)
            return;

        HandleTyping();
    }

    // =====================================
    // START QUICK MODE
    // =====================================

    public void StartQuickWordMode()
    {
        currentMode =
            TypingGameMode.QuickWords;

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

        targetText =
            Randomized_PromptRarity();
        timerScript.StartTimer();
        critLetters = null;
        ResetTyping();

        UpdateTextUI();

        Debug.Log("LONG PROMPT MODE");
    }

    // =====================================
    // GENERATE QUICK WORD
    // =====================================

    public void GenerateRandomWord()
    {
        int randomIndex =
            Random.Range(
                0,
                quickWords.Count
            );

        targetText =
            quickWords[randomIndex];

        ResetTyping();

        RollCritLettersForWord();

        UpdateTextUI();
    }

    void RollCritLettersForWord()
    {
        critLetters = new bool[targetText.Length];

        for (int i = 0; i < targetText.Length; i++)
        {
            critLetters[i] =
                rewardsSystem.RollCritLetter();
        }
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
    // INPUT
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
        }
    }

    // =====================================
    // TYPE CHARACTER
    // =====================================

    void TypeCharacter(char c)
    {
        if (
            currentIndex >=
            targetText.Length
        )
            return;

        char expectedChar =
            targetText[currentIndex];

        typedText += c;

        accuracySystem.RegisterInput(
            c,
            expectedChar
        );

        currentIndex++;

        // AUDIO
        if (c == expectedChar)
        {
            musicManager.PlayCorrectKeySFX();
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
        musicManager.FinishedWord();

        // ---------------------------------
        // QUICK WORD MODE
        // ---------------------------------

        if (
            currentMode ==
            TypingGameMode.QuickWords
        )
        {
            int reward =
                rewardsSystem
                .CalculateQuickReward(
                    targetText,
                    typedText,
                    accuracySystem.finalAccuracy,
                    critLetters
                );

            Debug.Log(
                "WORD COMPLETE +" +
                reward
            );
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

            bool isCritLetter =
                currentMode ==
                TypingGameMode.QuickWords &&
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
                    : $"<color=#777777>{targetChar}</color>";
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

        if (rarityRoll < 0.50f)
        {
            chosenRarity =
                rows.Common;

            currentPromptRarity =
                "Common";
        }
        else if (rarityRoll < 0.80f)
        {
            chosenRarity =
                rows.Uncommon;

            currentPromptRarity =
                "Uncommon";
        }
        else if (rarityRoll < 0.99f)
        {
            chosenRarity =
                rows.Rare;

            currentPromptRarity =
                "Rare";
        }
        else
        {
            chosenRarity =
                rows.Epic;

            currentPromptRarity =
                "Epic";
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