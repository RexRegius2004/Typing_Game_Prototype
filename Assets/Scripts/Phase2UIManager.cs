using UnityEngine;
using TMPro;

public class Phase2_UIManager : MonoBehaviour
{
    public TextMeshProUGUI currencyTextUI;
    public CurrencySystem currencySystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         UpdateCurrencyUI();
    }
    public void UpdateCurrencyUI()
    {
        currencyTextUI.text = $"${currencySystem.Money.ToString()}";
    }

}
