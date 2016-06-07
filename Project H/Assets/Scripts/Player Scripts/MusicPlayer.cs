using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource musicSource;

    public AudioClip[] musicClips;

    private int CurrentMusicIndex = 0;

    private bool playerStoppedMusic = false;

    void Start()
    {

        musicSource.clip = musicClips[0];
        musicSource.playOnAwake = false;
        musicSource.volume = 0.15f;
    }


    void Update()
    {

        if (!musicSource.isPlaying && !playerStoppedMusic)
        {
            CurrentMusicIndex = (CurrentMusicIndex + 1) % musicClips.Length;
            musicSource.clip = musicClips[CurrentMusicIndex];
            musicSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            if (musicSource.isPlaying)
            {
                musicSource.Stop();
                playerStoppedMusic = true;
            }

            else
            {
                musicSource.Play();
                playerStoppedMusic = false;
            }
        }

        else if (Input.GetKeyDown(KeyCode.F10))
        {
            if (CurrentMusicIndex == 0)
            {
                CurrentMusicIndex = musicClips.Length - 1;
            }
            else
            {
                CurrentMusicIndex -= 1;
            }

            musicSource.clip = musicClips[CurrentMusicIndex];
            musicSource.Play();
        }

        else if (Input.GetKeyDown(KeyCode.F12))
        {
            CurrentMusicIndex = (CurrentMusicIndex + 1) % musicClips.Length;
            musicSource.clip = musicClips[CurrentMusicIndex];
            musicSource.Play();
        }

    }
}
