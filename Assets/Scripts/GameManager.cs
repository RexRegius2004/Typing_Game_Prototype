using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public MusicManager musicManager;
    //public UpgradeManager UpgradeManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (musicManager == null)
        {
            musicManager = FindAnyObjectByType<MusicManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlayAgain()
    {
        //UpgradeManager.ResetUpgrades();
        musicManager.PlayButtonClickSFX();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        musicManager.PlayButtonClickSFX();
        Application.Quit();
    }
}
