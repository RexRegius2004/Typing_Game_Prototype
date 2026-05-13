using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public UpgradeManager UpgradeManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlayAgain()
    {
        UpgradeManager.ResetUpgrades();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
