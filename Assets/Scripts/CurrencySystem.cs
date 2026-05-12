using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    [Header("Money Settings")]
    public int Money = 0;

    void Start()
    {
        LoadMoney();
    }
    public void AddMoney(int money)
    {
        Money += money;
        SaveMoney(Money);
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
