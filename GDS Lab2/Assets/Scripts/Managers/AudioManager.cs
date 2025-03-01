using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NamedAudioClip
{
    public string clipName;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private NamedAudioClip[] clips;  // Array of NamedAudioClip

    private Dictionary<string, AudioClip> clipMap;    // Dictionary from string → AudioClip
    private AudioSource sfxSource;

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

        sfxSource = GetComponent<AudioSource>();

        clipMap = new Dictionary<string, AudioClip>();
        foreach (var item in clips)
        {
            if (!clipMap.ContainsKey(item.clipName))
            {
                clipMap.Add(item.clipName, item.clip);
            }
        }
    }

    public void PlaySFX(string clipName, float volume = 1f)
    {
        if (clipMap.ContainsKey(clipName))
        {
            sfxSource.PlayOneShot(clipMap[clipName], volume);
        }
        else
        {
            Debug.LogWarning($"Clip with name '{clipName}' not found in clipMap!");
        }
    }
}