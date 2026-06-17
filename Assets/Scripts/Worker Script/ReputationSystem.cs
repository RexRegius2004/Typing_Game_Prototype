using UnityEngine;

public class ReputationSystem : MonoBehaviour
{
    [Range(0, 100)]
    public float reputation = 50f;

    public void AddReputation(float amount)
    {
        reputation = Mathf.Clamp(reputation + amount, 0f, 100f);
    }

    public void SubtractReputation(float amount)
    {
        reputation = Mathf.Clamp(reputation - amount, 0f, 100f);
    }

    public float GetNormalizedReputation()
    {
        return reputation / 100f; 
    }
}
