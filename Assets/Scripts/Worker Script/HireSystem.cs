using UnityEngine;
using System.Collections.Generic;

public class HireSystem : MonoBehaviour
{
    public List<WorkerData> allWorkers;

    public ReputationSystem reputationSystem;

    public WorkerData RollWorker()
    {
        WorkerRarity rarity = RollRarity();
        List<WorkerData> pool = allWorkers.FindAll(w => w.rarity == rarity);

        if (pool.Count == 0)
        {
            Debug.LogWarning("No workers for rarity: " + rarity);
            return null;
        }

        return pool[Random.Range(0, pool.Count)];
    }

    WorkerRarity RollRarity()
    {
        float rep = reputationSystem.GetNormalizedReputation();

        // base chances
        float common = 50f - rep * 20f;
        float uncommon = 25f;
        float rare = 15f + rep * 10f;
        float epic = 8f + rep * 7f;
        float legendary = 2f + rep * 3f;

        float total = common + uncommon + rare + epic + legendary;
        float roll = Random.Range(0f, total);

        if (roll < common) return WorkerRarity.Common;
        roll -= common;

        if (roll < uncommon) return WorkerRarity.Uncommon;
        roll -= uncommon;

        if (roll < rare) return WorkerRarity.Rare;
        roll -= rare;

        if (roll < epic) return WorkerRarity.Epic;

        return WorkerRarity.Legendary;
    }
}
