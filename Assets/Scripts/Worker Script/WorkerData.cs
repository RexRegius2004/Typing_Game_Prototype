using UnityEngine;

[CreateAssetMenu(menuName = "Idle/Worker Data")]
public class WorkerData : ScriptableObject
{
    [Header("Identity")]
    public string workerName;
    public Sprite icon;
    public WorkerRarity rarity;

    [Header("Work Stats")]
    public float typingSpeed = 1f;
    public int reward = 10;
    [Range(0f, 1f)] public float successRate = 0.8f;

    [Header("Burnout")]
    [Range(0f, 1f)] public float burnoutChance = 0.05f;   
    public float burnoutTolerance = 0f;                  
    public float burnoutCapacity = 100f;                 
    public float recoveryRate = 10f;                    
}