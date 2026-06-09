using UnityEngine;

public class TestWorker : MonoBehaviour
{
    public WorkerData testWorker;
    public WorkerManager workerManager;

void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        workerManager.HireWorker(testWorker);
    }
}
}
