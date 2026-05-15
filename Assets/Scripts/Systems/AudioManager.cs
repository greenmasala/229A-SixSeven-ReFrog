using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager: MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public static AudioManager Instance;
    public float MasterVol;
    public float MusicVol;
    public float SFXVol;
    public AudioSource Music;
    public AudioSource SFX;

    [Header("Music")]
    public AudioClip BGM;

    [Header("UI")]
    public AudioClip Flicker;
    public AudioClip Loading;
    public AudioClip Typing;
    public AudioClip Popup;

    [Header("Player")]
    public AudioClip Jump;
    public AudioClip Yahoo;
    public AudioClip Walk;
    public AudioClip Refresh;
    public AudioClip Refresh2;
    public AudioClip Death;

    [Header("SFX")]
    public AudioClip Jumppad;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVol") & PlayerPrefs.HasKey("MusicVol") & PlayerPrefs.HasKey("SFXVol"))
        {
            LoadSettings();
            Debug.Log("Settings loaded");
        }
        else
        {
            Debug.Log("Default settings");
            SetMasterVol(0.75f);
            SetMusicVol(0.5f);
            SetSFXVol(0.5f);
        }

        Music.clip = BGM;
        Music.Play();
    }
    public void SetMasterVol(float vol)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(vol) * 20f);
        MasterVol = vol;
        PlayerPrefs.SetFloat("MasterVol", vol);
    }
    public void SetMusicVol(float vol)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(vol) * 20f);
        MusicVol = vol;
        PlayerPrefs.SetFloat("MusicVol", vol);
    }

    public void SetSFXVol(float vol)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(vol) * 20f);
        SFXVol = vol;
        PlayerPrefs.SetFloat("SFXVol", vol);
    }

    public void LoadSettings()
    {
        SetMasterVol(PlayerPrefs.GetFloat("MasterVol"));
        SetMusicVol(PlayerPrefs.GetFloat("MusicVol"));
        SetSFXVol(PlayerPrefs.GetFloat("SFXVol"));   
    }

    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }
}
