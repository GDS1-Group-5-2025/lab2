using UnityEngine;
using System.Collections.Generic;

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
        PlayMusic("ground");
    }

    public void PlayMusic(string musicName, float volume = 1f)
    {
        if (musicMap.ContainsKey(musicName))
        {
            musicSource.Stop();                    
            musicSource.volume = volume;           
            musicSource.clip = musicMap[musicName];
            musicSource.Play();                    
        }
        else
        {
            Debug.LogWarning($"Music clip '{musicName}' not found in MusicManager!");
        }
    }

    // Stop the current music track (if any) from playing.
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
