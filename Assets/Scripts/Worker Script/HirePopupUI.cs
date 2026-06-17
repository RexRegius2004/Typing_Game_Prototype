using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HirePopupUI : MonoBehaviour
{
    public GameObject panel;
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rarityText;
  

    void Start()
    {
        panel.SetActive(false);
    }
    public void Show(WorkerData worker)
    {
        panel.SetActive(true);

        icon.sprite = worker.icon;
        nameText.text = worker.workerName;
        rarityText.text = worker.rarity.ToString();
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
