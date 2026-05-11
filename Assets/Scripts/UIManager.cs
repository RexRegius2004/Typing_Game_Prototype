using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject gameOverUI;
    public TextMeshProUGUI gameOverTextUI;
    public TextMeshProUGUI accuracyTextUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OpenGameOverUI(bool Outcome)
    {
        if (Outcome)
            gameOverTextUI.text = "You Win Nice Job!";
        else
            gameOverTextUI.text = "Game Over Time's Up";
        gameOverUI.SetActive(true);
    }
}
