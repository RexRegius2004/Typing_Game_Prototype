using UnityEngine;
using TMPro;

public class AccuracySystem : MonoBehaviour
{
    [Header("Settings")]
    public float passThreshold = 60f;
    public float autoCorrectBonus = 5f;

    private int totalCharacters = 0;
    private int mistakeCount = 0;
    private bool hasFinalResult = false;

    [Header("Results")]
    public float finalAccuracy;

    //public UIManager uIManager;

    public void RegisterInput(char typedChar, char correctChar)
    {
        if (hasFinalResult) return;

        totalCharacters++;

        if (typedChar != correctChar)
        {
            mistakeCount++;
        }
    }

    public void CalculateFinalAccuracy()
    {
        if (hasFinalResult) return;

        hasFinalResult = true;

        if (totalCharacters == 0)
        {
            finalAccuracy = 0;
            //uIManager.AccuracyResultUI(finalAccuracy);
            return;
        }

        float rawAccuracy =
            ((float)(totalCharacters - mistakeCount) / totalCharacters) * 100f;

        // Auto-correct bonus
        finalAccuracy = rawAccuracy + autoCorrectBonus;

        // Clamp 0-100
        finalAccuracy = Mathf.Clamp(finalAccuracy, 0f, 100f);

        //uIManager.AccuracyResultUI(finalAccuracy);

        Debug.Log("Final Accuracy: " + finalAccuracy + "%");
    }

public void ResetAccuracy()
{
    totalCharacters = 0;
    mistakeCount = 0;
    finalAccuracy = 100f;
}
}

