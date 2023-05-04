using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathOfHero.Utilities;

[Serializable]
public class MusicEntry
{
    public AudioClip music;
    public float volume = 1;
    public float pitch = 1;
}

[RequireComponent(typeof(AudioSource))]
public class MusicManager : Singleton<MusicManager>
{
    private AudioSource audioSource;

    [SerializeField]
    private float fadeDuration = 1f;

    [SerializeField]
    private List<MusicEntry> musics = new List<MusicEntry>();

    // Start is called before the first frame update
    void Start()
        => TryGetComponent<AudioSource>(out audioSource);

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
            PlayMusic();
        else
        {
            if (audioSource.time >= audioSource.clip.length - fadeDuration)
                StopMusic();
        }
    }

    public void PlayMusic(MusicEntry musicEntry) // play specified
    {
        audioSource.clip = musicEntry.music;
        audioSource.pitch = musicEntry.pitch;
        audioSource.volume = 0f;
        audioSource.DOFade(musicEntry.volume, fadeDuration);
        audioSource.Play();
    }

    public void PlayMusic() // play random from musics list
    {
        if (musics.Count <= 0)
            return;

        PlayMusic(musics[UnityEngine.Random.Range(0, musics.Count)]);
    }

    public async void StopMusic()
    {
        await audioSource.DOFade(0, fadeDuration).AsyncWaitForCompletion();
        audioSource.Stop();
    }

    public void SetMusics(MusicEntry[] musics)
    {
        if (musics == null)
            return;

        this.musics = new List<MusicEntry>(musics);
    }
}
