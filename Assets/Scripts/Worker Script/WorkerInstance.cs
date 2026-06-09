using UnityEngine;
using System;

public class WorkerInstance
{
    public WorkerData data;

    public float currentProgress;

    private CurrencySystem currencySystem;

    public WorkerInstance(WorkerData data, CurrencySystem currencySystem)
    {
        this.data = data;
        this.currencySystem = currencySystem;
    }

    public void Tick(float deltaTime)
    {
        currentProgress += data.typingSpeed * deltaTime;

        if (currentProgress >= data.documentLength)
        {
            currentProgress = 0f;
            EarnMoney();
        }
    }

    void EarnMoney()
    {
        currencySystem.AddMoney(Mathf.RoundToInt(data.baseIncome));
    }
}