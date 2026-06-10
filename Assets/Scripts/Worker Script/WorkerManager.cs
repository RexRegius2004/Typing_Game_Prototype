using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorkerManager : MonoBehaviour
{
    public WorkerUI workerPrefab;
    public Transform gridParent;
    public GridLayoutGroup grid;

    public CurrencySystem currencySystem;

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

        // subtract spacing so it doesn't overflow
        float cellWidth = (totalWidth - (columns - 1) * spacingX) / columns;
        float cellHeight = (totalHeight - (rows - 1) * spacingY) / rows;

        float size = Mathf.Min(cellWidth, cellHeight);

        grid.cellSize = new Vector2(size, size);
    }
}
