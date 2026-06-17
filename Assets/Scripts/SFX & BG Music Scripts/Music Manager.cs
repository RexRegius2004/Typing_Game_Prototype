using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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

    private bool hasStarted = false;

private static MusicManager instance;
    [System.Obsolete]
    void Awake()
{

    transform.SetParent(null);

    if (instance != null && instance != this)
    {
        Destroy(gameObject);
        return;
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
    SceneManager.sceneLoaded += OnSceneLoaded;
    StartCoroutine(ApplyVolumesNextFrame());
}

    void Start()
    {
        if (hasStarted) return;
        hasStarted = true;

        PlayMusicForScene(
            SceneManager.GetActiveScene().name
        );
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    PlayMusicForScene(scene.name);
    StartCoroutine(ApplyVolumesNextFrame());
}

IEnumerator ApplyVolumesNextFrame()
{
    yield return null; // wait one frame after audio starts
    ApplySavedVolumes();
}

    void PlayMusicForScene(string sceneName)
{
    if (musicSource == null) return;
    
    if (sceneName == "MainMenu")
        musicSource.clip = MainmenuMusic;
    else
        musicSource.clip = gameplayMusic;

    musicSource.Play();
}

    public void ApplySavedVolumes()
{
    Debug.Log("ApplySavedVolumes called");

    if (PlayerPrefs.HasKey("MasterVolume"))
    {
        float master = PlayerPrefs.GetFloat("MasterVolume");
        Debug.Log("MasterVolume from PlayerPrefs: " + master);
        float safe = Mathf.Max(master, 0.0001f);
        bool result = audioMixer.SetFloat("MasterVolume", Mathf.Log10(safe) * 20);
        Debug.Log("SetFloat MasterVolume result: " + result);
    }
    else
    {
        Debug.Log("NO MasterVolume key found in PlayerPrefs");
    }

    if (PlayerPrefs.HasKey("MusicVolume"))
    {
        float music = PlayerPrefs.GetFloat("MusicVolume");
        Debug.Log("MusicVolume from PlayerPrefs: " + music);
        float safe = Mathf.Max(music, 0.0001f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(safe) * 20);
    }
    else
    {
        Debug.Log("NO MusicVolume key found in PlayerPrefs");
    }

    if (PlayerPrefs.HasKey("SFXVolume"))
    {
        float sfx = PlayerPrefs.GetFloat("SFXVolume");
        Debug.Log("SFXVolume from PlayerPrefs: " + sfx);
        float safe = Mathf.Max(sfx, 0.0001f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(safe) * 20);
    }
    else
    {
        Debug.Log("NO SFXVolume key found in PlayerPrefs");
    }
}

   void OnDestroy()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
    
    // Clear static instance if this is the one being destroyed
    if (instance == this)
        instance = null;
}

    public void PlayButtonClickSFX()
{
    if (sfxSource == null) return;
    sfxSource.PlayOneShot(buttonClickSFX);
}

public void PlayCorrectKeySFX()
{
    if (sfxSource == null) return;
    sfxSource.PlayOneShot(correctKeySFX);
}

public void PlayCriticalHitSFX()
{
    if (sfxSource == null) return;
    sfxSource.PlayOneShot(CriticalHitSFX);
    sfxSource.pitch = NormalPitch;
}

public void PlayIncorrectKeySFX()
{
    if (sfxSource == null) return;
    sfxSource.PlayOneShot(incorrectKeySFX);
}

public void FinishedWord()
{
    if (sfxSource == null) return;
    sfxSource.PlayOneShot(FinishedWordSFX[Random.Range(0, FinishedWordSFX.Length)]);
    sfxSource.pitch = NormalPitch;
}

public void RepeatGameSFX()
{
    if (sfxSource == null) return;
    sfxSource.PlayOneShot(MistakeQuickWordSFX);
    sfxSource.pitch = NormalPitch;
}

    internal void SwitchMusic(AudioClip gameplayMusic)
    {
        throw new System.NotImplementedException();
    }
}