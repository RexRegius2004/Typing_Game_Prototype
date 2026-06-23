using UnityEngine;

public class WorkerInstance
{
    public WorkerData data;
    private CurrencySystem currency;

    public float progress = 0f;

    // 🔥 Burnout runtime values
    public float burnout = 0f;
    public bool isBurnedOut = false;

    public WorkerInstance(WorkerData data, CurrencySystem currency)
    {
        this.data = data;
        this.currency = currency;
    }

    public void Tick(float dt)
    {
        if (isBurnedOut)
        {
            Recover(dt);
            return;
        }

        // 🔥 Roll burnout chance
        TryGainBurnout(dt);

        // ✍️ Work
        progress += data.typingSpeed * dt;

        if (progress >= 100f)
        {
            progress = 0f;
            CompleteWork();
        }
    }

    void CompleteWork()
    {
        // 🎯 Success roll
        if (Random.value <= data.successRate)
        {
            currency.AddMoney(data.reward);
        }
        else
        {
            Debug.Log(data.workerName + " failed the task.");
        }
    }

    void TryGainBurnout(float dt)
    {
        float finalChance = data.burnoutChance - data.burnoutTolerance;
        finalChance = Mathf.Max(0f, finalChance);

        if (Random.value < finalChance * dt)
        {
            burnout += 10f; 
            Debug.Log(data.workerName + "added burnout level");

            if (burnout >= data.burnoutCapacity)
            {
                EnterBurnout();
            }
        }
    }

    void EnterBurnout()
    {
        isBurnedOut = true;
        Debug.Log(data.workerName + " is burned out!");
    }

    void Recover(float dt)
    {
        burnout -= data.recoveryRate * dt;

        if (burnout <= 0f)
        {
            burnout = 0f;
            isBurnedOut = false;
            Debug.Log(data.workerName + " recovered!");
        }
    }

    public float GetProgressPercent()
    {
        return progress / 100f;
    }

    public bool IsBurnedOut()
    {
        return isBurnedOut;
    }

    public float GetBurnoutPercent()
    {
        return burnout / data.burnoutCapacity;
    }
}