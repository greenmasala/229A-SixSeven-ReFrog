using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider SFXSlider;
    public TextMeshProUGUI SFXVal;
    public Slider MusicSlider;
    public TextMeshProUGUI MusicVal;
    public Slider MasterSlider;
    public TextMeshProUGUI MasterVal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateText();
   
        SFXSlider.onValueChanged.AddListener(GetSFXVol);
        MusicSlider.onValueChanged.AddListener(GetMusicVol);
        MasterSlider.onValueChanged.AddListener(GetMasterVol);
    }

    void GetSFXVol(float val)
    {
        AudioManager.Instance.SetSFXVol(val);
        var percentage = (val / 1) * 100;
        SFXVal.text = percentage.ToString("F0");
    }
    void GetMusicVol(float val)
    {
        AudioManager.Instance.SetMusicVol(val);
        var percentage = (val / 1) * 100;
        MusicVal.text = percentage.ToString("F0");
    }
    void GetMasterVol(float val)
    {
        AudioManager.Instance.SetMasterVol(val);
        var percentage = (val / 1) * 100;
        MasterVal.text = percentage.ToString("F0");
    }

    void UpdateText()
    {
        SFXSlider.value = AudioManager.Instance.SFXVol;
        var percentage = (AudioManager.Instance.SFXVol / 1) * 100;
        SFXVal.text = percentage.ToString("F0");
        Debug.Log(percentage);

        MusicSlider.value = AudioManager.Instance.MusicVol;
        var percentage2 = (AudioManager.Instance.MusicVol / 1) * 100;
        MusicVal.text = percentage2.ToString("F0");
        Debug.Log(percentage2);

        MasterSlider.value = AudioManager.Instance.MasterVol;
        var percentage3 = (AudioManager.Instance.MasterVol / 1) * 100;
        MasterVal.text = percentage3.ToString("F0");
        Debug.Log(percentage3);
    }
}
