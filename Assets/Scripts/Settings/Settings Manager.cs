using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public MusicManager musicManager;

[Header("Game Object References ")]
    [SerializeField] private GameObject settingsPanel;
     [SerializeField] private GameObject volumePanel;

[Header("Volume Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    private float volume;

    void Awake()
    {
        if (musicManager == null)
        {
            musicManager = FindAnyObjectByType<MusicManager>();
        }

    }

    void Start()
    {
        settingsPanel.SetActive(false);
        volumePanel.SetActive(false);

    if (PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("MasterVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
         SetMasterVolume();
         SetMusicVolume();
         SetSFXVolume();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
                Time.timeScale = 0f; // Pause the game when settings are open
            }
        }
    }

    public void OpenSettings()
    {
        musicManager.PlayButtonClickSFX();
        settingsPanel.SetActive(true);
    }

    public void OpenVolumeSettings()
    {
        musicManager.PlayButtonClickSFX();
        volumePanel.SetActive(true);
    }

    public void CloseSettings()
    {
        musicManager.PlayButtonClickSFX();
        settingsPanel.SetActive(false);
        volumePanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game when settings are closed
    }

    public void SetMasterVolume()
    {
        volume = masterSlider.value;
        musicManager.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume()
    {
        volume = musicSlider.value;
        musicManager.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume()
    {
        volume = sfxSlider.value;
        musicManager.audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMusicVolume();
        SetMasterVolume();
        SetSFXVolume();
    }

    public void BacktoMainMenu(string sceneName)
    {
        musicManager.PlayButtonClickSFX();
        Time.timeScale = 1f; // Ensure the game is not paused when returning to main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
