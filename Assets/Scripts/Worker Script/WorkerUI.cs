using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkerUI : MonoBehaviour
{
    [Header("UI References")]
    public Image icon;
    public TextMeshProUGUI progressText;

    [Header("Sliders")]
    public Slider progressSlider;
    public Slider burnoutSlider;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color burnedOutColor = Color.red;

    private WorkerInstance worker;

    public WorkerUI Bind(WorkerInstance worker)
    {
        this.worker = worker;

        if (worker.data.icon != null)
            icon.sprite = worker.data.icon;

        return this;
    }

    void Update()
    {
        if (worker == null) return;

        float progress = Mathf.Clamp01(worker.GetProgressPercent());
        float burnout = Mathf.Clamp01(worker.GetBurnoutPercent());

        bool burnedOut = worker.IsBurnedOut();

        // 🧠 TEXT
        if (burnedOut)
        {
            progressText.text = "Burned Out";
            progressText.color = burnedOutColor;
        }
        else
        {
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
            progressText.color = normalColor;
        }

        // 📊 SLIDERS
        if (progressSlider != null)
            progressSlider.value = progress;

        if (burnoutSlider != null)
            burnoutSlider.value = burnout;
    }
}
