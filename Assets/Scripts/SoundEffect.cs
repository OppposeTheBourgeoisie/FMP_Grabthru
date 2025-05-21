using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

[System.Serializable]
public class SoundEffect
{
    public string Name;
    public AudioClip[] Clips;
    public AudioMixerGroup MixerGroup;

    public bool RandomizePitch = false;
    [Range(0.5f, 2.0f)] public float MinPitch = 0.9f;
    [Range(0.5f, 2.0f)] public float MaxPitch = 1.1f;

    public bool Loop = false; // Enable/disable looping
    public float LoopDuration = 5f; // Duration to loop in seconds

    public AudioClip GetRandomClip()
    {
        if (Clips == null || Clips.Length == 0)
            return null;
        return Clips[Random.Range(0, Clips.Length)];
    }

    public float GetPitch()
    {
        return RandomizePitch ? Random.Range(MinPitch, MaxPitch) : 1f;
    }
}