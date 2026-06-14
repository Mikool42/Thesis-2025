using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using SmallHedge.SoundManager;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(SoundManager.MASTER_KEY, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(SoundManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(SoundManager.SFX_KEY, 1f);
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(SoundManager.MASTER_KEY, masterSlider.value);
        PlayerPrefs.SetFloat(SoundManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(SoundManager.SFX_KEY, sfxSlider.value);
    }

    void SetMasterVolume(float value)
    {
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
    }
    
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    
    void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
}
