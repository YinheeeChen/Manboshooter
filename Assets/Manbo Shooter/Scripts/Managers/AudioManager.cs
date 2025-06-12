using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool IsSFXOn { get; private set; }
    public bool IsMusicOn { get; private set; }
    private int lastIndex = -1;
    private AudioSource[] audioSources;
    private AudioSource currentMusicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);

        SettingsManager.onSFXStateChanged += SFXStateChangedCallback;
        SettingsManager.onMusicStateChanged += MusicStateChangedCallback;

        audioSources = GetComponentsInChildren<AudioSource>();
    }

    private void OnDestroy()
    {
        SettingsManager.onSFXStateChanged -= SFXStateChangedCallback;
        SettingsManager.onMusicStateChanged -= MusicStateChangedCallback;
    }

    private void Update()
    {
        if (!currentMusicSource.isPlaying)
        {
            PlayNextMusic();
        }
    }

    private void SFXStateChangedCallback(bool sfxState)
    {
        IsSFXOn = sfxState;
    }

    private void MusicStateChangedCallback(bool musicState)
    {
        IsMusicOn = musicState;

        // AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();

        if (musicState)
        {
            lastIndex = (lastIndex + 1) % audioSources.Length;
            for (int i = 0; i < audioSources.Length; i++)
            {
                if (i == lastIndex)
                {
                    audioSources[i].mute = false;
                    audioSources[i].Stop();
                    audioSources[i].Play();
                    currentMusicSource = audioSources[i];
                }
                else
                {
                    audioSources[i].Stop();
                    audioSources[i].mute = true;
                }
            }
        }
        else
        {

            foreach (var audioSource in audioSources)
            {
                audioSource.Stop();
                audioSource.mute = true;
            }
        }
    }
    
    private void PlayNextMusic()
    {
        if (audioSources == null || audioSources.Length == 0)
            return;

        lastIndex = (lastIndex + 1) % audioSources.Length;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (i == lastIndex)
            {
                currentMusicSource = audioSources[i];
                currentMusicSource.mute = false;
                currentMusicSource.Stop();
                currentMusicSource.Play();
            }
            else
            {
                audioSources[i].Stop();
                audioSources[i].mute = true;
            }
        }
    }
}
