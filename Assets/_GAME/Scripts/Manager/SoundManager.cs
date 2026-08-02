using UnityEditor.Timeline.Actions;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    [Header("Audio Source")]

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioSource sfxSource;


    [Header("Audio Clip")]

    [SerializeField] private AudioClip bgmGame;

    [SerializeField] private AudioClip bgmWin;

    [SerializeField] private AudioClip bgmFail;

    [SerializeField] private AudioClip buttonClick;

    [SerializeField] private AudioClip impact;

    [SerializeField] private AudioClip collectBrick;

    [SerializeField] private AudioClip buildBridge;

    [SerializeField] private AudioClip buyButton;

    [SerializeField] private AudioClip changeCoin;


    public void PlayMusic(AudioClip audioClip)
    {
        if(musicSource == null) return;

        musicSource.clip = audioClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip audioClip)
    {
        if(sfxSource == null) return;

        sfxSource.PlayOneShot(audioClip);
    }

    public void PlayBG()
    {
        PlayMusic(bgmGame);
    }

    public void PlayWinMusic()
    {
        PlayMusic(bgmWin);
    }

    public void PlayFailMusic()
    {
        PlayMusic(bgmFail);
    }

    public void PlaySFXClick()
    {
        PlaySfx(buttonClick);
    }
    public void PlaySFXImpact()
    {
        PlaySfx(impact);
    }

    public void PlaySFXCollectBrick()
    {
        PlaySfx(collectBrick);
    }

    public void PlaySFXBuildBridge()
    {
        PlaySfx(buildBridge);
    }

    public void PlaySFXEarnCoin()
    {
        PlaySfx(changeCoin);
    }

    public void PlaySFXBuy()
    {
        PlaySfx(buyButton);
    }




}
