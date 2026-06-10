using UnityEngine;

public class TestWorker : MonoBehaviour
{
    public WorkerData common;
    public WorkerData uncommon;
    public WorkerData rare;
    public WorkerData epic;
    public WorkerData Legendary;
    public WorkerManager workerManager;

    public GameObject debugPanel;

    void Start()
    {
        CloseDebugPanel();
    }
    void Update()
    {
    
    }

    public void OpenDebugPanel()
    {
        debugPanel.SetActive(true);
    }

    public void CloseDebugPanel()
    {
        debugPanel.SetActive(false);
    }
    public void AddCommonWorker()
    {
    workerManager.HireWorker(common);
    }

    public void AddUncommonWorker()
    {
    workerManager.HireWorker(uncommon);
    }
    public void AddRareWorker()
    {
    workerManager.HireWorker(rare);
    }

    public void AddEpicWorker()
    {
    workerManager.HireWorker(epic);
    }

    public void AddLegendaryWorker()
    {
    workerManager.HireWorker(Legendary);
    }
}
