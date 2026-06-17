using UnityEngine;
using TMPro;
public class ShopManager : MonoBehaviour
{
    public WorkerManager workerManager;
    public CurrencySystem currencySystem;
    public GameObject ShopUI;
    public TextMeshProUGUI Price;
    public int WorkerPrice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShopUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Price.text = $"${WorkerPrice.ToString()}";
    }

    public void HireWorker()
    {
        currencySystem.SubtractMoney(WorkerPrice);
        workerManager.HireRandomWorker();
    }

    public void CloseShop()
    {
        ShopUI.SetActive(false);
    }

    public void OpenShop()
    {
        ShopUI.SetActive(true);
    }
}
