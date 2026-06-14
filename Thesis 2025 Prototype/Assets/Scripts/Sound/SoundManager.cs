//Author: Small Hedge Games
//Updated: 13/06/2024

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SoundManager
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsSO SO;
        private static SoundManager instance = null;
        private AudioSource audioSource;

        ///////////////////////////////////////////////////////////////////////////////
        // Made for this project not from the smallHedge codebase
        [SerializeField] AudioMixer mixer;

        public const string MASTER_KEY = "masterVolume";
        public const string MUSIC_KEY = "musicVolume";
        public const string SFX_KEY = "sfxVolume";
        ///////////////////////////////////////////////////////////////////////////////

        private void Awake()
        {
            if(!instance)
            {
                instance = this;
                audioSource = GetComponent<AudioSource>();
            }

            ///////////////////////////////////////////////////////////////////////////////
            // Made for this project not from the smallHedge codebase
            LoadVolume();
            ///////////////////////////////////////////////////////////////////////////////
        }

    public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

            if(source)
            {
                source.outputAudioMixerGroup = soundList.mixer;
                source.clip = randomClip;
                source.volume = volume * soundList.volume;
                source.Play();
            }
            else
            {
                instance.audioSource.outputAudioMixerGroup = soundList.mixer;
                instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////
        // Made for this project not from the smallHedge codebase
        void LoadVolume()
        {
            float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
            float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

            mixer.SetFloat(SoundSettings.MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
            mixer.SetFloat(SoundSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
            mixer.SetFloat(SoundSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
        }
        ///////////////////////////////////////////////////////////////////////////////
    }

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }
}