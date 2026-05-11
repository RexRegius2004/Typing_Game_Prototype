
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

void Awake()
    {
       
    }
    void Start()
    {
        Prompt_Tier = GameObject.Find("Prompt_Random").GetComponent<Prompt_Rarity>();
        targetTextUI.text = Randomized_PromptRarity();
        typedTextUI.text = "";
    }

    void Update()
    {
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

    void CheckFinished()
    {
        if (typedText == targetText)
        {
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

   
}
