
using UnityEngine;
using TMPro;
using Mono.Cecil.Cil;


public class TypingController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI targetTextUI;
    public TextMeshProUGUI typedTextUI;

    [Header("Typing Settings")]
    [TextArea(3, 5)]
    public string targetText = "The quick brown fox jumps over the lazy dog.";
    private string typedText = "";
    private int correctIndex = 0;

     [Header("Prompt Rarity")]
    public Prompt_Rarity Prompt_Tier;

 [Header("Timer")]
    public TimerScript timerScript;

    private bool isGameActive = true;

    [Header("UI")]
    public UIManager uIManager;

    [Header("Accuracy System")]
    public AccuracySystem accuracySystem;

    void Start()
    {
        Prompt_Tier = GameObject.Find("Prompt_Random").GetComponent<Prompt_Rarity>();
        targetTextUI.text = Randomized_PromptRarity();
        typedTextUI.text = "";
        timerScript.OnTimerEnd += HandleTimeUp;
    }
    void Update()
    {
        if (!isGameActive) return;
        InputTyping();
    }
    
    void InputTyping()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (typedText.Length > 0)
                {
                    typedText = typedText.Substring(0, typedText.Length - 1);

                    correctIndex = 0;
                    for (int i = 0; i < typedText.Length; i++)
                    {
                        if (i < targetText.Length && typedText[i] == targetText[i])
                            correctIndex++;
                        else
                            break;
                    }
                }
            }

            else if (c == '\n' || c == '\r')
            {
                continue;
            }
            else
            {
                if (typedText.Length < targetText.Length)
                {
                    typedText += c;
                    if (typedText.Length <= targetText.Length)
{
                    char expectedChar = targetText[typedText.Length - 1];
                    accuracySystem.RegisterInput(c, expectedChar);
}

                    
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
            if (i < targetText.Length && typedText[i] == targetText[i])
            {
                result += $"<color=white>{typedText[i]}</color>";
            }
            else
            {
                result += $"<color=red>{typedText[i]}</color>";
            }
        }

        typedTextUI.text = result;
    }

    void ShowFinalMistakes()
    {
        string result = "";

        for (int i = 0; i < targetText.Length; i++)
        {
            if (i < typedText.Length && typedText[i] == targetText[i])
            {
                result += $"<color=white>{targetText[i]}</color>";
            }
            else
            {
                result += $"<color=red>{targetText[i]}</color>";
            }
        }

    typedTextUI.text = result;
    }

    void CheckFinished()
    {
        if (typedText == targetText)
        {
            isGameActive = false;
            timerScript.StopTimer();
            accuracySystem.CalculateFinalAccuracy();
            uIManager.OpenGameOverUI(true);
            Debug.Log("You win!");
        }
    }

#region Prompt Rarity
    public string Randomized_PromptRarity()
    {
        //Random row from array is chosen
        var allRows = Prompt_Tier.promptList.PromptRarity;
        int randomIndex = Random.Range(0, allRows.Length);
        var rows = allRows[randomIndex];


        //Roll Rarity
        float rarityRoll = Random.value;
        string chosenRarity = rows.Common;

        if (rarityRoll < 0.5f) //50%
        {
            chosenRarity = rows.Common;
        }
        else if (rarityRoll < 0.3f) //30%
        {
            chosenRarity = rows.Uncommon;
        }
        else if (rarityRoll < 0.1f) //10%
        {
            chosenRarity = rows.Rare;
        }
        else if (rarityRoll < 0.5f) //5%
        {
            chosenRarity = rows.Epic;
        }
        else if (rarityRoll < 0.001f) //0.1%
        {
            chosenRarity = rows.Legendary;
        }

        return chosenRarity;
    }
    
#endregion

   
    void HandleTimeUp()
    {
        isGameActive = false;
        ShowFinalMistakes();
        //accuracySystem.CalculateFinalAccuracy();
        uIManager.OpenGameOverUI(false);
        Debug.Log("You Lose! Time ran out.");
    }   
}
