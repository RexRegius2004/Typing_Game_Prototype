using Unity.VisualScripting;
using UnityEngine;

public class RewardsSystem : MonoBehaviour
{
    [Header("References")]
    public TypingController typingController;
    public AccuracySystem accuracySystem;
    public TimerScript timerScript;
    public CurrencySystem currencySystem;

    [Header("Reward Settings")]

    [Tooltip("Flat reward added every game")]
    public int baseReward = 1;

    [Tooltip("Speed bonus per remaining second")]
    public float speedValue = 0.05f;

    [Header("Difficulty Multipliers")]
    public float commonMultiplier = 1f;
    public float uncommonMultiplier = 1.05f;
    public float rareMultiplier = 1.1f;
    public float epicMultiplier = 1.25f;
    public float legendaryMultiplier = 1.5f;

    [Header("Results")]
    public int finalMoney;
    public int wordsTyped;
    public int correctWords;
    public float accuracy;
    public float remainingTime;
    public float difficultyMultiplier;

    public void CalculateRewards()
    {
        // =====================================
        // GET VALUES
        // =====================================

        accuracy = accuracySystem.finalAccuracy;
        remainingTime = timerScript.GetRemainingTime();

        // Total words in prompt
        wordsTyped = CountWords(typingController.targetText);

        // Accuracy-adjusted correct words
        correctWords = Mathf.RoundToInt(
            wordsTyped * (accuracy / 100f)
        );

        // =====================================
        // DIFFICULTY MULTIPLIER
        // =====================================

        difficultyMultiplier = GetDifficultyMultiplier(
            typingController.currentPromptRarity
        );

        // =====================================
        // WORD REWARD (SQRT SCALING)
        // Prevents long prompts from exploding economy
        // =====================================

        float wordReward =
            Mathf.Sqrt(correctWords);

        // =====================================
        // SPEED BONUS
        // =====================================

        float speedBonus =
            remainingTime * speedValue;

        // =====================================
        // SUBTOTAL
        // =====================================

        float subtotal =
            baseReward +
            wordReward +
            speedBonus;

        // =====================================
        // SOFT ACCURACY MULTIPLIER
        // Less punishing than pure accuracy/100
        // =====================================

        float accuracyMultiplier =
            0.5f + (accuracy / 200f);

        subtotal *= accuracyMultiplier;

        // =====================================
        // DIFFICULTY MULTIPLIER
        // =====================================

        subtotal *= difficultyMultiplier;

        // =====================================
        // FINAL REWARD
        // =====================================

        finalMoney = Mathf.FloorToInt(subtotal);

        // Minimum reward floor
        finalMoney = Mathf.Max(finalMoney, 1);

        // =====================================
        // GIVE MONEY
        // =====================================

        currencySystem.AddMoney(finalMoney);

        // =====================================
        // DEBUG
        // =====================================

        Debug.Log("===== REWARD BREAKDOWN =====");
        Debug.Log("Words Typed: " + wordsTyped);
        Debug.Log("Correct Words: " + correctWords);
        Debug.Log("Accuracy: " + accuracy);
        Debug.Log("Remaining Time: " + remainingTime);
        Debug.Log("Word Reward: " + wordReward);
        Debug.Log("Speed Bonus: " + speedBonus);
        Debug.Log("Difficulty Multiplier: " + difficultyMultiplier);
        Debug.Log("Final Reward: " + finalMoney);
    }

    int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        string[] words = text.Split(' ');

        return words.Length;
    }

    float GetDifficultyMultiplier(string rarity)
    {
        switch (rarity)
        {
            case "Common":
                return commonMultiplier;

            case "Uncommon":
                return uncommonMultiplier;

            case "Rare":
                return rareMultiplier;

            case "Epic":
                return epicMultiplier;

            case "Legendary":
                return legendaryMultiplier;

            default:
                return 1f;
        }
    }
}
