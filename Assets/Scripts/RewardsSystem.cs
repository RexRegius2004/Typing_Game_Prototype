using Unity.VisualScripting;
using UnityEngine;

public class RewardsSystem : MonoBehaviour
{
      [Header("References")]
    public TypingController typingController;
    public AccuracySystem accuracySystem;
    public TimerScript timerScript;
    public CurrencySystem currencySystem;
    public UpgradeManager upgradeManager;

    [Header("Reward Settings")]

    [Tooltip("Flat reward added every game")]
    public int baseReward = 10;

    [Tooltip("Reward per correctly typed word")]
    public float wordValue = 0.5f;

    [Tooltip("Reward per second remaining")]
    public float speedValue = 0.25f;

    [Header("Difficulty Multipliers")]
    public float commonMultiplier = 1f;
    public float uncommonMultiplier = 1.1f;
    public float rareMultiplier = 1.25f;
    public float epicMultiplier = 1.5f;
    public float legendaryMultiplier = 2f;

    [Header("Results")]
    public int finalMoney;
    public int wordsTyped;
    public int correctCharacters;
    public float accuracy;
    public float remainingTime;
    public float difficultyMultiplier;

    [Header("Critical Hits")]
    public int criticalMoney;

    public void AddCriticalReward(int amount)
    {
        criticalMoney += amount;
    }
    public void CalculateRewards()
    {
        // GET VALUES
        accuracy = accuracySystem.finalAccuracy;
        remainingTime = timerScript.GetRemainingTime();

        // Count words from target text
        wordsTyped = CountWords(typingController.targetText);

        // Count correctly typed characters
        correctCharacters = Mathf.RoundToInt(
            typingController.targetText.Length * (accuracy / 100f)
        );

        // DIFFICULTY MULTIPLIER
        difficultyMultiplier = GetDifficultyMultiplier(
            typingController.currentPromptRarity
        );

        // SCORE COMPONENTS
        float wordReward =
            wordsTyped * wordValue;

        float speedBonus =
            remainingTime * speedValue;

        float subtotal =
            baseReward +
            wordReward +
            speedBonus;

    
        // ACCURACY MULTIPLIER
        subtotal *= (accuracy / 100f);

        // FINAL MULTIPLIER
        subtotal *= difficultyMultiplier;

        // FINAL REWARD
        finalMoney = Mathf.FloorToInt(subtotal) + criticalMoney + upgradeManager.currentBonus;

        // MINIMUM REWARD
        finalMoney = Mathf.Max(finalMoney, 10);

        
        currencySystem.AddMoney(finalMoney);
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
