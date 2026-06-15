using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource Master;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    public AudioMixer audioMixer;

    [Header("SFX")]
    public AudioClip buttonClickSFX;
    public AudioClip correctKeySFX;
    public AudioClip incorrectKeySFX;
    public AudioClip CriticalHitSFX;
    public AudioClip[] FinishedWordSFX;
    public AudioClip MistakeQuickWordSFX;

    [Header("Music")]
    public AudioClip MainmenuMusic;
    public AudioClip gameplayMusic;
    
    

[Header("Pitch")]
public float NormalPitch = 1f;

    [System.Obsolete]
    void Awake()
{
    // Singleton - persist across scenes
    if (FindObjectsByType<MusicManager>(FindObjectsSortMode.None).Length > 1)
    {
        Destroy(gameObject);
        return;
    }

    DontDestroyOnLoad(gameObject);

    // Apply saved volumes BEFORE any audio plays
    if (PlayerPrefs.HasKey("MasterVolume"))
    {
        float master = Mathf.Max(PlayerPrefs.GetFloat("MasterVolume"), 0.0001f);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(master) * 20);
    }

    if (PlayerPrefs.HasKey("MusicVolume"))
    {
        float music = Mathf.Max(PlayerPrefs.GetFloat("MusicVolume"), 0.0001f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(music) * 20);
    }

    if (PlayerPrefs.HasKey("SFXVolume"))
    {
        float sfx = Mathf.Max(PlayerPrefs.GetFloat("SFXVolume"), 0.0001f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfx) * 20);
    }
}

void Start()
{
    // Play the right music per scene
    string currentScene = 
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    if (currentScene == "MainMenu") // replace with your actual scene name
    {
        musicSource.clip = MainmenuMusic;
    }
    else
    {
        musicSource.clip = gameplayMusic;
    }

    musicSource.Play();
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonClickSFX()
    {
        sfxSource.PlayOneShot(buttonClickSFX);
    }

    public void PlayCorrectKeySFX()
    {
        sfxSource.PlayOneShot(correctKeySFX);
        
    }

    public void PlayCriticalHitSFX()
    {
        sfxSource.PlayOneShot(CriticalHitSFX);
        sfxSource.pitch = NormalPitch;
    }

    public void PlayIncorrectKeySFX()
    {
        sfxSource.PlayOneShot(incorrectKeySFX);
    }

    public void FinishedWord()
    {
        sfxSource.PlayOneShot(FinishedWordSFX[Random.Range(0, FinishedWordSFX.Length)]);
        sfxSource.pitch = NormalPitch;
    }

    public void RepeatGameSFX()
    {
        sfxSource.PlayOneShot(MistakeQuickWordSFX);
        sfxSource.pitch = NormalPitch;
    }
}
