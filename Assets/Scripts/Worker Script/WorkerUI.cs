using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class WorkerUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI progressText;

    private WorkerInstance worker;

    public WorkerUI Bind(WorkerInstance worker)
    {
        this.worker = worker;
        icon.sprite = worker.data.icon;
        return this;
    }

    void Update()
    {
        if (worker == null) return;

        float percent = worker.currentProgress / worker.data.documentLength;
        progressText.text = Mathf.RoundToInt(percent * 100f) + "%";
    }
}
