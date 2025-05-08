using System;
using UnityEngine;

public class UISoundsManager : MonoBehaviour
{
    [SerializeField] private float _soundVolume;
    [SerializeField] private AudioClip _clickSound;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        _audioSource.PlayOneShot(_clickSound, _soundVolume);
    }

    public void PlaySound(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip, _soundVolume);
    }
}
