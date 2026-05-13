
using UnityEngine;
using TMPro;

public class TypingController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI targetTextUI;
    public TextMeshProUGUI typedTextUI;

    [Header("Typing Settings")]
    [TextArea(3, 5)]
    public string targetText;

    private string typedText = "";

    [Header("Chunk Settings")]
    public int wordsPerChunk = 8;

    private string[] promptWords;
    private int currentChunkIndex = 0;
    private string currentTargetChunk = "";

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

    [Header("UI")]
    public UIManager uIManager;

    [Header("Accuracy System")]
    public AccuracySystem accuracySystem;

    [Header("Reward System")]
    public RewardsSystem rewardsSystem;

    [HideInInspector]
    public string currentPromptRarity { get; private set; } = "Common";

    void Start()
    {
        Prompt_Tier = GameObject.Find("Prompt_Random")
            .GetComponent<Prompt_Rarity>();

        targetText = Randomized_PromptRarity();

        promptWords = targetText.Split(' ');

        LoadNextChunk();

        timerScript.OnTimerEnd += HandleTimeUp;
    }

    void Update()
    {
        if (!isGameActive)
            return;

        HandleCaretBlink();
        InputTyping();
    }

    void HandleCaretBlink()
    {
        caretTimer += Time.deltaTime;

        if (caretTimer >= caretBlinkSpeed)
        {
            caretTimer = 0f;
            caretVisible = !caretVisible;

            UpdateTypedTextUI();
        }
    }

    void InputTyping()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (typedText.Length > 0)
                {
                    typedText = typedText.Substring(
                        0,
                        typedText.Length - 1
                    );
                }
            }
            else if (c == '\n' || c == '\r')
            {
                continue;
            }
            else
            {
                if (typedText.Length < currentTargetChunk.Length)
                {
                    typedText += c;

                    char expectedChar =
                        currentTargetChunk[typedText.Length - 1];

                    accuracySystem.RegisterInput(c, expectedChar);
                }
            }
        }

        UpdateTypedTextUI();
        CheckFinished();
    }

    void UpdateTypedTextUI()
    {
        string result = "";

        for (int i = 0; i < typedText.Length; i++)
        {
            if (
                i < currentTargetChunk.Length &&
                typedText[i] == currentTargetChunk[i]
            )
            {
                result +=
                    $"<color=white>{typedText[i]}</color>";
            }
            else
            {
                result +=
                    $"<color=red>{typedText[i]}</color>";
            }
        }

        // BLINKING CARET
        if (showCaret && caretVisible)
        {
            result += "<color=white>|</color>";
        }

        typedTextUI.text = result;
    }

    void LoadNextChunk()
    {
        typedText = "";

        int startIndex =
            currentChunkIndex * wordsPerChunk;

        // Finished entire prompt
        if (startIndex >= promptWords.Length)
        {
            FinishGame();
            return;
        }

        int endIndex = Mathf.Min(
            startIndex + wordsPerChunk,
            promptWords.Length
        );

        currentTargetChunk = "";

        for (int i = startIndex; i < endIndex; i++)
        {
            currentTargetChunk += promptWords[i];

            if (i < endIndex - 1)
            {
                currentTargetChunk += " ";
            }
        }

        targetTextUI.text = currentTargetChunk;

        UpdateTypedTextUI();
    }

    void CheckFinished()
    {
        if (typedText == currentTargetChunk)
        {
            currentChunkIndex++;

            LoadNextChunk();
        }
    }

    void FinishGame()
    {
        isGameActive = false;

        timerScript.StopTimer();

        accuracySystem.CalculateFinalAccuracy();

        rewardsSystem.CalculateRewards();

        uIManager.OpenGameOverUI(true);

        Debug.Log("You win!");
    }

    void ShowFinalMistakes()
    {
        string result = "";

        for (int i = 0; i < currentTargetChunk.Length; i++)
        {
            if (
                i < typedText.Length &&
                typedText[i] == currentTargetChunk[i]
            )
            {
                result +=
                    $"<color=white>{currentTargetChunk[i]}</color>";
            }
            else
            {
                result +=
                    $"<color=red>{currentTargetChunk[i]}</color>";
            }
        }

        typedTextUI.text = result;
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
            Debug.Log(rarityRoll);
        }
        else if (rarityRoll < 0.80f)
        {
            chosenRarity = rows.Uncommon;
            currentPromptRarity = "Uncommon";
            Debug.Log(rarityRoll);
        }
        else if (rarityRoll < 0.99f)
        {
            chosenRarity = rows.Rare;
            currentPromptRarity = "Rare";
            Debug.Log(rarityRoll);
        }
        else if (rarityRoll < 1f)
        {
            chosenRarity = rows.Epic;
            currentPromptRarity = "Epic";
            Debug.Log(rarityRoll);
        }
        else
        {
            chosenRarity = rows.Legendary;
            currentPromptRarity = "Legendary";
            Debug.Log(rarityRoll);
        }

        return chosenRarity;
    }

#endregion

    void HandleTimeUp()
    {
        isGameActive = false;

        ShowFinalMistakes();

        uIManager.OpenGameOverUI(false);

        Debug.Log("You Lose! Time ran out.");
    }
}
