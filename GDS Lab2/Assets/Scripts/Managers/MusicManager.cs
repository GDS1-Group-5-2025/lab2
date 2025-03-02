using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

[System.Serializable]
public class NamedMusicClip
{
    public string clipName;    
    public AudioClip clip;     
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField]
    private NamedMusicClip[] musicClips;
    private Dictionary<string, AudioClip> musicMap;

    private AudioSource musicSource;

    [Header("Environment State")]
    [SerializeField] private bool isUnderground = false;
    [SerializeField] private bool isHurry = false;
    [SerializeField] private bool isInvincible = false;

    private string lastLoopingTrack = "ground";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;

        musicMap = new Dictionary<string, AudioClip>();
        foreach (var item in musicClips)
        {
            if (!musicMap.ContainsKey(item.clipName))
            {
                musicMap.Add(item.clipName, item.clip);
            }
        }
    }

    private void Start()
    {
        EvaluateMusicState();
    }

    public void EvaluateMusicState()
    {
        // If invincible, always play "invincibility" track, overriding others
        if (isInvincible)
        {
            PlayLoopingMusic("invincibility");
            return;
        }

        // If hurry, choose "groundHurry" or "undergroundHurry"
        if (isHurry)
        {
            if (isUnderground)
                PlayLoopingMusic("underground hurry");
            else
                PlayLoopingMusic("ground hurry");
        }
        else
        {
            // Normal pace, choose "ground" or "underground"
            if (isUnderground)
                PlayLoopingMusic("underground");
            else
                PlayLoopingMusic("ground");
        }
    }

    public void StartInvincibility()
    {
        isInvincible = true;
        EvaluateMusicState();
    }

    public void StopInvincibility()
    {
        isInvincible = false;
        EvaluateMusicState();
    }

    public void SetUnderground(bool value)
    {
        isUnderground = value;
        EvaluateMusicState();
    }

    public void SetHurry(bool value)
    {
        isHurry = value;
        EvaluateMusicState();
    }

    private void PlayLoopingMusic(string musicName, float volume = 1f)
    {
        if (musicMap.ContainsKey(musicName))
        {
            lastLoopingTrack = musicName; // remember what we're playing
            musicSource.Stop();
            musicSource.volume = volume;
            musicSource.clip = musicMap[musicName];
            musicSource.loop = true;       // ensure loop is on
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music clip '{musicName}' not found!");
        }
    }

    private void ResumeLastLoopingTrack()
    {
        // Simply call PlayLoopingMusic again with lastLoopingTrack
        PlayLoopingMusic(lastLoopingTrack);
    }

    public void PlayNonLoopingClipThenRevert(string musicName, float volume = 1f)
    {
        if (!musicMap.ContainsKey(musicName))
        {
            Debug.LogWarning($"Non-looping clip '{musicName}' not found!");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(PlayNonLoopingCoroutine(musicName, volume));
    }

    private IEnumerator PlayNonLoopingCoroutine(string clipName, float volume)
    {
        musicSource.Stop();
        musicSource.loop = false;
        musicSource.volume = volume;
        musicSource.clip = musicMap[clipName];
        musicSource.Play();

        // Wait for the clip to finish playing
        yield return new WaitForSeconds(musicSource.clip.length);

        // After the non-looping clip finishes, go back to last looping track
        ResumeLastLoopingTrack();
    }

    // Stop the current music track (if any) from playing.
    public void StopNonLoopingClipAndRevert()
    {
        StopAllCoroutines();
        musicSource.Stop();
        ResumeLastLoopingTrack();
    }

}
