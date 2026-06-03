using UnityEngine;

[CreateAssetMenu(fileName = "WorkerUpgradeData", menuName = "Scriptable Objects/WorkerUpgradeData")]
public class WorkerUpgradeData : ScriptableObject
{
    public string upgradeName;
    
     [Header("Stat Bonuses Per Level")]
    public float Proficiency;
    public float Equipment;
    public float Burnout_Recover;
    public float Burnout_Tolerance;
    public float Burnout_Capacity;
    public float Worker_CritChance;
    public float Worker_CritHit;
    public float Success_Rate;
}
