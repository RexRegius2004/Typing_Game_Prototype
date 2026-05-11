using UnityEngine;
using TMPro;

public class AccuracySystem : MonoBehaviour
{
    [Header("Settings")]
    public float passThreshold = 60f;
    public float autoCorrectBonus = 5f;

    [Header("UI")]
    public TextMeshProUGUI accuracyTextUI;

    private string resultTextUI;

    private int totalCharacters = 0;
    private int mistakeCount = 0;

    private bool hasFinalResult = false;

    // Call this every time player types
    public void RegisterInput(char typedChar, char correctChar)
    {
        if (hasFinalResult) return;

        totalCharacters++;

        if (typedChar != correctChar)
        {
            mistakeCount++;
        }
    }

    // FINAL CALCULATION (call on win or lose)
    public void CalculateFinalAccuracy()
    {
        if (hasFinalResult) return;

        hasFinalResult = true;

        if (totalCharacters == 0)
        {
            ShowResult(0);
            return;
        }

        float rawAccuracy = ((float)(totalCharacters - mistakeCount) / totalCharacters) * 100f;

        // Apply auto-correct bonus
        float finalAccuracy = rawAccuracy + autoCorrectBonus;

        // Clamp to 100%
        finalAccuracy = Mathf.Clamp(finalAccuracy, 0f, 100f);

        ShowResult(finalAccuracy);
    }

    void ShowResult(float accuracy)
    {

        if (accuracy < passThreshold)
        {
            resultTextUI = "No Rewards";
            Debug.Log("Failed - No rewards");
        }
        else
        {
            resultTextUI = "Give Rewards";
            Debug.Log("Passed - Give rewards");
        }

         accuracyTextUI.text = $"Accuracy: {accuracy:0}% \n {resultTextUI}";
    }
}
