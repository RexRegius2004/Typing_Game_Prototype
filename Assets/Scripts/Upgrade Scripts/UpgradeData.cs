using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
     public string upgradeName;
    public int maxLevel;

    [Header("Stat Bonuses Per Level")]
    public float critChance;
    public float critHit;

}
