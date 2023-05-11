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
public class AudioManager : Singleton<AudioManager>
{
    private AudioSource audioSource;

    [SerializeField] [Tooltip("Unit: seconds")]
    private float fadeDuration = 1f;

    [SerializeField]
    private List<MusicEntry> musics = new List<MusicEntry>();

    private int lastMusicIndex = -1;

    private bool stopRequested = false;

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
            if (!stopRequested && (audioSource.time >= audioSource.clip.length - fadeDuration))
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

        // prevent playing the same music again after it has finished
        int random;
        do
            random = UnityEngine.Random.Range(0, musics.Count);
        while (musics.Count > 1 && random == lastMusicIndex);

        PlayMusic(musics[random]);
        lastMusicIndex = random;
    }

    public async void StopMusic()
    {
        stopRequested = true;
        await audioSource.DOFade(0, fadeDuration).AsyncWaitForCompletion();
        audioSource.Stop();
    }

    public void SetMusics(MusicEntry[] musics)
    {
        if (musics == null)
            return;

        this.musics = new List<MusicEntry>(musics);
        lastMusicIndex = -1;
    }

    public void PlaySound(AudioClip sound)
        => audioSource.PlayOneShot(sound);
}
