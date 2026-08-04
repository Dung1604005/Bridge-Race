using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum AudioClipType
{
    BGM_GAMEPLAY = 0,
    BGM_WIN = 1,
    BGM_LOSE = 2,
    SFX_BUTTON_CLICK = 3,

    SFX_BRICK_DROP = 4,

    SFX_BRICK_IMPACT = 5,

    SFX_BRICK_TAKE = 6,

    SFX_BUILD_BRIDGE = 7

}

public enum AudioSourceType
{
    MUSIC = 0,
    SFX = 1
}

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] soundAus;


    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }

    public void PlayMusicSound(AudioClipType audioClipType)
    {
        if(musicSource == null|| GameData.Instance.PlayerData.IsMute) return;

        musicSource.clip = soundAus[(int)audioClipType];
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySfx(AudioClipType audioClipType)
    {
        if(sfxSource == null || GameData.Instance.PlayerData.IsMute) return;

        sfxSource.PlayOneShot(soundAus[(int)audioClipType]);
    }

   


    

    

}