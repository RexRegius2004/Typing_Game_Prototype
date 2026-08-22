using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    [Header("Money Settings")]
    public int Money = 0;

    [SerializeField] RandomSpawner moneySpawner;

    void Start()
    {
        LoadMoney();
        if (moneySpawner == null)
            moneySpawner = FindAnyObjectByType<RandomSpawner>();
    }

    public void AddMoney(int money)
    {
        Money += money;
        SaveMoney(Money);

        if (money > 0 && moneySpawner != null)
            moneySpawner.Spawn(money);
    }

    public void SubtractMoney(int money)
    {
        Money -= money;
        SaveMoney(Money);
    }

    public void ResetMoney()
    {
        Money = 0;
        SaveMoney(0);
    }

    public void SaveMoney(int money)
    {
        PlayerPrefs.SetInt("Money", money);
    }

    public void LoadMoney()
    {
        Money = PlayerPrefs.GetInt("Money");
    }
}
