using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource Master;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Clips")]
    public AudioClip buttonClickSFX;
    public AudioClip correctKeySFX;
    public AudioClip incorrectKeySFX;
    public AudioClip MainmenuMusic;
    public AudioClip gameplayMusic;

    void Start()
    {
        // Set initial music
        musicSource.clip = gameplayMusic;
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

    public void PlayIncorrectKeySFX()
    {
        sfxSource.PlayOneShot(incorrectKeySFX);
    }
}
