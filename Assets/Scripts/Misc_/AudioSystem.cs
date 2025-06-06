using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    //Call "AudioSystem.Instance.PlaySound("soundName")" to play the specified sound effect
    public static AudioSystem Instance;

    public AudioMixer MainMixer;

    [Header("Sound Effects")]
    public List<SoundEffect> SoundEffects;

    private Dictionary<string, SoundEffect> soundDict;
    private AudioSource sfxSource;

    void Awake()
    {
        // Setup singleton, dictionary, and audio source
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        soundDict = new Dictionary<string, SoundEffect>();
        foreach (var sfx in SoundEffects)
        {
            if (!soundDict.ContainsKey(sfx.Name))
                soundDict.Add(sfx.Name, sfx);
        }

        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        // Play a sound effect by name
        if (soundDict.TryGetValue(name, out SoundEffect sfx))
        {
            AudioClip clip = sfx.GetRandomClip();

            GameObject tempGO = new GameObject("TempAudio");
            AudioSource tempSource = tempGO.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.outputAudioMixerGroup = sfx.MixerGroup;
            tempSource.Play();
            Destroy(tempGO, clip.length);
        }
    }

    public void SetVolume(string exposedParam, float linearVolume)
    {
        // Set the volume of an exposed mixer parameter
        float dB = Mathf.Log10(Mathf.Clamp(linearVolume, 0.0001f, 1f)) * 20f;
        MainMixer.SetFloat(exposedParam, dB);
    }
}
