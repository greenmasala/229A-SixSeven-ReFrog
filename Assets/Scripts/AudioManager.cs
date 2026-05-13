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
    public AudioClip Typing;
    public AudioClip Popup;
    public AudioClip LevelComplete;

    [Header("Player")]
    public AudioClip Jump;
    public AudioClip Refresh;
    //public Slider MasterSlider;
    //public Slider MusicSlider;
    //public Slider SFXSlider;
    //public TextMeshProUGUI MasterVolVal;
    //public TextMeshProUGUI MusicVolVal;
    //public TextMeshProUGUI SFXVolVal;

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
        //var percentage = (vol / 1) * 100;
        //MasterVolVal.text = percentage.ToString("F0");
        PlayerPrefs.SetFloat("MasterVol", vol);
        //MasterSlider.value = PlayerPrefs.GetFloat("MasterVol");
    }
    public void SetMusicVol(float vol)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(vol) * 20f);
        MusicVol = vol;
        //var percentage = (vol / 1) * 100;
        //MusicVolVal.text = percentage.ToString("F0");
        PlayerPrefs.SetFloat("MusicVol", vol);
        //MusicSlider.value = PlayerPrefs.GetFloat("MusicVol");
    }

    public void SetSFXVol(float vol)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(vol) * 20f);
        SFXVol = vol;
        //var percentage = (vol / 1) * 100;
        //SFXVolVal.text = percentage.ToString("F0");
        PlayerPrefs.SetFloat("SFXVol", vol);
        //SFXSlider.value = PlayerPrefs.GetFloat("SFXVol");
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
