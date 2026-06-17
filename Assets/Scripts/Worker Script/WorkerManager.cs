using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorkerManager : MonoBehaviour
{
    public WorkerUI workerPrefab;
    public Transform gridParent;
    public GridLayoutGroup grid;

    public CurrencySystem currencySystem;
    public HireSystem hireSystem;
    public HirePopupUI popup;

    private List<WorkerInstance> workers = new List<WorkerInstance>();

    void Update()
    {
        float dt = Time.deltaTime;

        foreach (var w in workers)
            w.Tick(dt);
    }

    public void HireWorker(WorkerData data)
    {
        var instance = new WorkerInstance(data, currencySystem);
        workers.Add(instance);

        Instantiate(workerPrefab, gridParent)
            .Bind(instance);

        UpdateGrid();
    }

    public void HireRandomWorker()
    {
        WorkerData worker = hireSystem.RollWorker();

        if (worker == null)
        {
            Debug.LogWarning("No worker rolled!");
            return;
        }

        // Add to your system
        HireWorker(worker);

        // Show popup if exists
        if (popup != null)
        {
            popup.Show(worker);
        }

        Debug.Log("Hired: " + worker.workerName + " (" + worker.rarity + ")");
    }

    void UpdateGrid()
    {
        int count = workers.Count;

        int columns = Mathf.CeilToInt(Mathf.Sqrt(count));
        int rows = Mathf.CeilToInt((float)count / columns);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        RectTransform rect = grid.GetComponent<RectTransform>();

        float totalWidth = rect.rect.width;
        float totalHeight = rect.rect.height;

        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        float cellWidth = (totalWidth - (columns - 1) * spacingX) / columns;
        float cellHeight = (totalHeight - (rows - 1) * spacingY) / rows;

        float size = Mathf.Min(cellWidth, cellHeight);

        grid.cellSize = new Vector2(size, size);
    }
}
