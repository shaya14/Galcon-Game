using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SoundFx : Singleton<SoundFx>
{

    public AudioSource _audioSource;
    public AudioClip _clickSound;
    public AudioClip _selectSound;
    public AudioClip _attackSound;
    public AudioClip _hitSound;
    public AudioClip _conquerSound;
    public AudioClip _winSound;
    public AudioClip _gameOverSound;
    public AudioClip _gameStartSound;
    public AudioClip _gameRestartSound;
    public AudioClip _gamePauseSound;
    public AudioClip _gameResumeSound;
    public AudioClip _gameQuitSound;
    public AudioClip _gameOverMusic;
    public AudioClip _gameMusic;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        _audioSource.PlayOneShot(clip);
        _audioSource.volume = volume;
    }

    public void PlaySound(AudioClip clip, float volume, float pitch)
    {
        _audioSource.PlayOneShot(clip);
        _audioSource.volume = volume;
        _audioSource.pitch = pitch;
    }
}
