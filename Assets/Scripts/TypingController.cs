
using UnityEngine;
using TMPro;

public class TypingController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI targetTextUI;

    [Header("Typing Settings")]
    [TextArea(3, 10)]
    public string targetText;

    private string typedText = "";
    private int currentIndex = 0;

    [Header("Caret Settings")]
    public bool showCaret = true;
    public float caretBlinkSpeed = 0.5f;

    private bool caretVisible = true;
    private float caretTimer = 0f;

    [Header("Prompt Rarity")]
    public Prompt_Rarity Prompt_Tier;

    [Header("Timer")]
    public TimerScript timerScript;

    private bool isGameActive = true;

    [Header("Systems")]
    public UIManager uIManager;
    public AccuracySystem accuracySystem;
    public RewardsSystem rewardsSystem;
    public UpgradeManager upgradeManager;

    [Header("Critical Hit")]
    private bool[] criticalLetters;

    [HideInInspector]
    public string currentPromptRarity { get; private set; } = "Common";

    void Start()
    {
        Prompt_Tier = GameObject.Find("Prompt_Random")
            .GetComponent<Prompt_Rarity>();

        targetText = Randomized_PromptRarity();

        GenerateCriticalLetters();

        UpdateTextUI();

        timerScript.OnTimerEnd += HandleTimeUp;
    }

    void Update()
    {
        if (!isGameActive)
            return;

        //HandleCaretBlink();

        InputTyping();
    }

    // =====================================
    // CARET BLINK
    // =====================================

    void HandleCaretBlink()
    {
        caretTimer += Time.deltaTime;

        if (caretTimer >= caretBlinkSpeed)
        {
            caretTimer = 0f;

            caretVisible = !caretVisible;

            UpdateTextUI();
        }
    }

    // =====================================
    // TYPING INPUT
    // =====================================

    void InputTyping()
    {
        foreach (char c in Input.inputString)
        {
            // BACKSPACE
            if (c == '\b')
            {
                if (typedText.Length > 0)
                {
                    typedText =
                        typedText.Substring(
                            0,
                            typedText.Length - 1
                        );

                    currentIndex--;
                }
            }

            // IGNORE ENTER
            else if (c == '\n' || c == '\r')
            {
                continue;
            }

            // NORMAL INPUT
            else
            {
                if (currentIndex < targetText.Length)
                {
                    char expectedChar =
                        targetText[currentIndex];

                    typedText += c;

                    accuracySystem.RegisterInput(
                        c,
                        expectedChar
                    );

                    CheckCriticalHit(
                        c,
                        expectedChar
                    );

                    currentIndex++;
                }
            }
        }

        UpdateTextUI();

        // FINISH
        if (typedText.Length >= targetText.Length)
        {
            FinishGame();
        }
    }

    // =====================================
    // UPDATE TEXT UI
    // =====================================

    void UpdateTextUI()
    {
        string result = "";

        for (int i = 0; i < targetText.Length; i++)
        {
            char targetChar = targetText[i];

            // =====================================
            // TYPED CHARACTERS
            // =====================================

            if (i < typedText.Length)
            {
                if (typedText[i] == targetChar)
                {
                    result +=
                        $"<color=white>{targetChar}</color>";
                }
                else
                {
                    result +=
                        $"<color=red>{targetChar}</color>";
                }
            }

            // =====================================
            // CURRENT CHARACTER
            // =====================================

            else if (i == currentIndex)
            {
                // Critical Letter
                if (
                    criticalLetters != null &&
                    criticalLetters[i]
                )
                {
                    result +=
                        $"<mark=#FFD70088><color=#FFD700>{targetChar}</color></mark>";
                }
                else
                {
                    result +=
                        $"<mark=#FFFFFF44>{targetChar}</mark>";
                }

                /*
                if (showCaret && caretVisible)
                {
                    result += "<color=white>|</color>";
                }
                */
            }

            // =====================================
            // UNTOUCHED CHARACTERS
            // =====================================

            else
            {
                // Critical Letter
                if (
                    criticalLetters != null &&
                    criticalLetters[i]
                )
                {
                    result +=
                        $"<color=#FFD700>{targetChar}</color>";
                }
                else
                {
                    result +=
                        $"<color=#777777>{targetChar}</color>";
                }
            }
        }

        targetTextUI.text = result;

    }

    // =====================================
    // CRITICAL LETTERS
    // =====================================

    void GenerateCriticalLetters()
    {
        criticalLetters =
            new bool[targetText.Length];

        for (int i = 0; i < targetText.Length; i++)
        {
            // Ignore spaces
            if (targetText[i] == ' ')
                continue;

            criticalLetters[i] =
                Random.value <=
                upgradeManager.currentCritChance;
        }
    }

    void CheckCriticalHit(
        char typedChar,
        char expectedChar
    )
    {
        int index = currentIndex;

        if (
            index < 0 ||
            index >= criticalLetters.Length
        )
            return;

        // Must be critical letter
        if (!criticalLetters[index])
            return;

        // Must type correctly
        if (typedChar != expectedChar)
            return;

        int critReward = Mathf.Max(
            1,
            Mathf.RoundToInt(
                rewardsSystem.wordValue *
                upgradeManager.currentCritHit
            )
        );

        rewardsSystem.AddCriticalReward(
            critReward
        );

        Debug.Log(
            "CRITICAL HIT! +" + critReward
        );
    }

    // =====================================
    // FINISH GAME
    // =====================================

    void FinishGame()
    {
        isGameActive = false;

        timerScript.StopTimer();

        accuracySystem.CalculateFinalAccuracy();

        rewardsSystem.CalculateRewards();

        uIManager.OpenGameOverUI(true);

        Debug.Log("You win!");
    }

    // =====================================
    // SHOW FINAL ERRORS
    // =====================================

    void ShowFinalMistakes()
    {
        string result = "";

        for (int i = 0; i < targetText.Length; i++)
        {
            if (
                i < typedText.Length &&
                typedText[i] == targetText[i]
            )
            {
                result +=
                    $"<color=white>{targetText[i]}</color>";
            }
            else
            {
                result +=
                    $"<color=red>{targetText[i]}</color>";
            }
        }

        targetTextUI.text = result;
    }

#region Prompt Rarity

    public string Randomized_PromptRarity()
    {
        if (
            Prompt_Tier == null ||
            Prompt_Tier.promptList == null ||
            Prompt_Tier.promptList.PromptRarity == null ||
            Prompt_Tier.promptList.PromptRarity.Length == 0
        )
        {
            Debug.LogError(
                "Prompt_Tier reference is missing!"
            );

            return "Error: No Prompt Rarity";
        }

        // Random row
        var allRows =
            Prompt_Tier.promptList.PromptRarity;

        int randomIndex =
            Random.Range(0, allRows.Length);

        var rows = allRows[randomIndex];

        // Roll rarity
        float rarityRoll = Random.value;

        string chosenRarity = rows.Common;

        if (rarityRoll < 0.50f)
        {
            chosenRarity = rows.Common;
            currentPromptRarity = "Common";
        }
        else if (rarityRoll < 0.80f)
        {
            chosenRarity = rows.Uncommon;
            currentPromptRarity = "Uncommon";
        }
        else if (rarityRoll < 0.99f)
        {
            chosenRarity = rows.Rare;
            currentPromptRarity = "Rare";
        }
        else if (rarityRoll < 1f)
        {
            chosenRarity = rows.Epic;
            currentPromptRarity = "Epic";
        }
        else
        {
            chosenRarity = rows.Legendary;
            currentPromptRarity = "Legendary";
        }

        return chosenRarity;
    }

#endregion

    // =====================================
    // TIMER END
    // =====================================

    void HandleTimeUp()
    {
        isGameActive = false;

        ShowFinalMistakes();

        uIManager.OpenGameOverUI(false);

        Debug.Log("You Lose! Time ran out.");
    }
}