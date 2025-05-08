using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float musicVolume;
    [SerializeField] private float fadeDuration = 1f;

    private bool isFading;

    private void Start()
    {
        PlayRandomMusic();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !isFading)
        {
            StartCoroutine(PlayNextMusicWithFade());
        }
    }

    private void PlayRandomMusic()
    {
        int randomIndex = Random.Range(0, musicClips.Length);
        AudioClip randomClip = musicClips[randomIndex];
        audioSource.clip = randomClip;
        audioSource.volume = musicVolume; 
        audioSource.Play();
    }

    private IEnumerator PlayNextMusicWithFade()
    {
        isFading = true;

        float startVolume = audioSource.volume;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.Stop();

        int randomIndex = Random.Range(0, musicClips.Length);
        AudioClip randomClip = musicClips[randomIndex];
        audioSource.clip = randomClip;
        audioSource.Play();

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = musicVolume; 
        isFading = false;
    }
}
