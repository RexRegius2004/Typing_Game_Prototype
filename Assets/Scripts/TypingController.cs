
using UnityEngine;
using TMPro;

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

    void Start()
    {
        targetTextUI.text = targetText;
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
}
