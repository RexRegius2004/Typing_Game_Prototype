using UnityEngine;

[CreateAssetMenu(fileName = "WorkerData", menuName = "Scriptable Objects/WorkerData")]
public class WorkerData : ScriptableObject
{
    [Header("Identity")]
    public string workerName;
    public Sprite icon;

    [Header("Typing Stats")]
    public float typingSpeed = 5f; // characters per second
    public float documentLength = 100f;

    [Header("Economy")]
    public float baseIncome = 10f;
}