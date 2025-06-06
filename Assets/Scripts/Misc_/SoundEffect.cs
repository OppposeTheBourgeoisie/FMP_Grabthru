using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

[System.Serializable]
public class SoundEffect
{
    // Class for audio files that go into a list in the Audio System
    public string Name;
    public AudioClip[] Clips;
    public AudioMixerGroup MixerGroup;

    public bool RandomizePitch = false;
    [Range(0.5f, 2.0f)] public float MinPitch = 0.9f;
    [Range(0.5f, 2.0f)] public float MaxPitch = 1.1f;

    public bool Loop = false;
    public float LoopDuration = 5f;

    public AudioClip GetRandomClip()
    {
        // Plays a random clip from the array of clips
        if (Clips == null || Clips.Length == 0)
            return null;
        return Clips[Random.Range(0, Clips.Length)];
    }

    public float GetPitch()
    {
        // Randomizes the pitch if enabled, otherwise it's always 1
        return RandomizePitch ? Random.Range(MinPitch, MaxPitch) : 1f;
    }
}