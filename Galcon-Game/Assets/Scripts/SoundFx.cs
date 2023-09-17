using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SoundFx : Singleton<SoundFx>
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _clickSound;
    [SerializeField] AudioClip _selectSound;
    [SerializeField] AudioClip _attackSound;
    [SerializeField] AudioClip _hitSound;
    [SerializeField] AudioClip _conquerSound;
    [SerializeField] AudioClip _winSound;
    [SerializeField] AudioClip _gameOverSound;
    [SerializeField] AudioClip _gameStartSound;
    [SerializeField] AudioClip _gameRestartSound;
    [SerializeField] AudioClip _gamePauseSound;
    [SerializeField] AudioClip _gameResumeSound;
    [SerializeField] AudioClip _gameQuitSound;
    [SerializeField] AudioClip _gameOverMusic;
    [SerializeField] AudioClip _gameMusic;

    public AudioClip clickSound => _clickSound;
    public AudioClip selectSound => _selectSound;
    public AudioClip attackSound => _attackSound;
    public AudioClip hitSound => _hitSound;
    public AudioClip conquerSound => _conquerSound;
    public AudioClip winSound => _winSound;
    public AudioClip gameOverSound => _gameOverSound;
    public AudioClip gameStartSound => _gameStartSound;
    public AudioClip gameRestartSound => _gameRestartSound;
    public AudioClip gamePauseSound => _gamePauseSound;
    public AudioClip gameResumeSound => _gameResumeSound;
    public AudioClip gameQuitSound => _gameQuitSound;
    public AudioClip gameOverMusic => _gameOverMusic;
    public AudioClip gameMusic => _gameMusic;

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
