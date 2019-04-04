using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClass : MonoBehaviour
{
    private AudioSource _audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        _audioSource.pitch = 0.8f;
        Play();
    }

    public void StopMusic()
    {
        _audioSource.Pause();
    }

    public void PlayReverse()
    {
        _audioSource.pitch = -0.8f;
        Play();
    }

    private void Play()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }
}
