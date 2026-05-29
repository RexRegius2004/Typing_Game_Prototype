using UnityEngine;

public class RewardsSystem : MonoBehaviour
{
    [Header("References")]
    public CurrencySystem currencySystem;
    public UpgradeManager upgradeManager;

    [Header("Quick Word Rewards")]
    public int minimumQuickReward = 1;

    [Tooltip("Money per character")]
    public float letterValue = 1f;

    [Header("Long Prompt Rewards")]
    public int baseReward = 10;
    public float wordValue = 0.5f;
    public float speedValue = 0.25f;

    [Header("Difficulty Multipliers")]
    public float commonMultiplier = 1f;
    public float uncommonMultiplier = 1.1f;
    public float rareMultiplier = 1.25f;
    public float epicMultiplier = 1.5f;
    public float legendaryMultiplier = 2f;

    [Header("Results")]
    public int finalMoney;
    public float difficultyMultiplier;

    [Header("Critical Rewards")]
    public int criticalMoney;
    internal object wordsTyped;

// =====================================
// LAST RUN RESULTS (UI SAFE)
// =====================================

[Header("Last Run Results (UI)")]
public int lastWordsTyped;
public float lastAccuracy;
public float lastRemainingTime;
public float lastDifficultyMultiplier;
    // =====================================
    // QUICK WORD REWARD
    // =====================================

    public bool RollCritLetter()
    {
        if (
            upgradeManager == null ||
            upgradeManager.currentCritChance <= 0f
        )
            return false;

        return Random.value <=
            upgradeManager.currentCritChance;
    }

    public int CalculateQuickReward(
        string completedWord,
        string typed,
        float accuracy,
        bool[] critLetters
    )
    {
        // ---------------------------------
        // BASE REWARD (per letter, crit letters pay more)
        // ---------------------------------

        float reward = 0f;
        criticalMoney = 0;
        float critMultiplier =
            upgradeManager.GetCritMultiplier();
        float accuracyScale = accuracy / 100f;

        for (int i = 0; i < completedWord.Length; i++)
        {
            float letterReward = letterValue;

            bool isCritLetter =
                critLetters != null &&
                i < critLetters.Length &&
                critLetters[i];

            bool typedCorrect =
                typed != null &&
                i < typed.Length &&
                typed[i] == completedWord[i];

            if (isCritLetter && typedCorrect)
            {
                float critBonus =
                    letterReward *
                    (critMultiplier - 1f);

                letterReward += critBonus;
                criticalMoney +=
                    Mathf.RoundToInt(
                        critBonus * accuracyScale
                    );
            }

            reward += letterReward;
        }

        // ---------------------------------
        // ACCURACY MULTIPLIER
        // ---------------------------------

        reward *= accuracyScale;

        // ---------------------------------
        // WAGE BONUS
        // ---------------------------------

        reward +=
            upgradeManager.currentWageBonus;

        if (criticalMoney > 0)
        {
            Debug.Log(
                "CRITICAL LETTERS! +" +
                criticalMoney
            );
        }

        // ---------------------------------
        // CONSISTENCY BONUS
        // ---------------------------------

        reward *=
            upgradeManager.ConsistencyBonus();

        // ---------------------------------
        // MINIMUM REWARD
        // ---------------------------------

        int finalReward =
            Mathf.Max(
                minimumQuickReward,
                Mathf.RoundToInt(reward)
            );

        // ---------------------------------
        // GIVE MONEY
        // ---------------------------------

        currencySystem.AddMoney(finalReward);

        return finalReward;
    }

    // =====================================
    // LONG PROMPT REWARD
    // =====================================

    public void CalculateLongPromptReward(
        TypingController typingController,
        AccuracySystem accuracySystem,
        TimerScript timerScript
        
        
    )
    {
        
        float accuracy =
            accuracySystem.finalAccuracy;

        float remainingTime =
            timerScript.GetRemainingTime();

        int wordsTyped =
            CountWords(
                typingController.targetText
            );

        difficultyMultiplier =
            GetDifficultyMultiplier(
                typingController.currentPromptRarity
            );

        float subtotal =
            baseReward;

        subtotal +=
            wordsTyped * wordValue;

        subtotal +=
            remainingTime * speedValue;

        // ACCURACY
        subtotal *=
            (accuracy / 100f);

        // DIFFICULTY
        subtotal *=
            difficultyMultiplier;

        // UPGRADES
        subtotal +=
            upgradeManager.currentWageBonus;

        subtotal +=
            upgradeManager.AheadofSchedule();

        subtotal *=
            upgradeManager.ConsistencyBonus();

        lastWordsTyped = wordsTyped;
        lastAccuracy = accuracy;
        lastRemainingTime = remainingTime;
        lastDifficultyMultiplier = difficultyMultiplier;
        // FINAL
        finalMoney =
            Mathf.RoundToInt(subtotal);

        finalMoney += criticalMoney;

        finalMoney =
            Mathf.Max(finalMoney, 10);

        currencySystem.AddMoney(finalMoney);

        Debug.Log(
            "LONG PROMPT PAYOUT: " +
            finalMoney
        );
    }

    // =====================================
    // COUNT WORDS
    // =====================================

    int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return text.Split(' ').Length;
    }

    // =====================================
    // DIFFICULTY MULTIPLIER
    // =====================================

    float GetDifficultyMultiplier(
        string rarity
    )
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