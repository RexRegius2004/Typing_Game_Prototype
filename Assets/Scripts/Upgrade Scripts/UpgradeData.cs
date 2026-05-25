using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
     public string upgradeName;

    [Header("Stat Bonuses Per Level")]
    public float critChance;
    public float critHit;
    public int wageBonus;
    public float aheadSchedMultiplier;
    public int delaytacticbonus;
    public float consistencyBonusMultiplier;
}
