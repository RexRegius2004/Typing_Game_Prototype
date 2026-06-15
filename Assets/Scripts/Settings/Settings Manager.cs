using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public MusicManager musicManager;

    [Header("Game Object References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject volumePanel;

    [Header("Volume Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    private float volume;

    private bool isInitializing = false;

    void Start()
    {
        if (musicManager == null)
            musicManager = FindAnyObjectByType<MusicManager>();

        settingsPanel.SetActive(false);
        volumePanel.SetActive(false);

        isInitializing = true;

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value  = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value    = PlayerPrefs.GetFloat("SFXVolume", 1f);

        isInitializing = false;

        isInitializing = false;

Debug.Log("Slider values after load — Master: " + masterSlider.value + 
          " | Music: " + musicSlider.value + 
          " | SFX: " + sfxSlider.value);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
                CloseSettings();
            else
            {
                OpenSettings();
                Time.timeScale = 0f;
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
        Time.timeScale = 1f;
    }

    public void SetMasterVolume()
{
    if (isInitializing)
    {
        Debug.Log("BLOCKED by isInitializing");
        return;
    }
    if (musicManager == null) return;
    
    volume = masterSlider.value;
    Debug.Log("SAVING MasterVolume: " + volume); // add this
    
    float safeVolume = Mathf.Max(volume, 0.0001f);
    musicManager.audioMixer.SetFloat("MasterVolume", Mathf.Log10(safeVolume) * 20);
    PlayerPrefs.SetFloat("MasterVolume", volume);
    PlayerPrefs.Save();
    
    Debug.Log("SAVED. Verify: " + PlayerPrefs.GetFloat("MasterVolume")); // and this
}

    public void SetMusicVolume()
    {
        if (isInitializing) return;
        if (musicManager == null) return;
        volume = musicSlider.value;
        float safeVolume = Mathf.Max(volume, 0.0001f);
        musicManager.audioMixer.SetFloat("MusicVolume", Mathf.Log10(safeVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        if (isInitializing) return;
        if (musicManager == null) return;
        volume = sfxSlider.value;
        float safeVolume = Mathf.Max(volume, 0.0001f);
        musicManager.audioMixer.SetFloat("SFXVolume", Mathf.Log10(safeVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void BacktoMainMenu(string sceneName)
    {
        musicManager.PlayButtonClickSFX();
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}