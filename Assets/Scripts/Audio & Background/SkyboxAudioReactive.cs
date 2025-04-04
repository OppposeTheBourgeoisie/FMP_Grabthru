using UnityEngine;
using System.Collections;

public class SkyboxAudioReactive : MonoBehaviour
{
    private AudioSource MusicSource;
    public Material SkyboxMaterial;
    public Color LowVolumeColor = new Color(0.2f, 0.0f, 0.5f);
    public Color HighVolumeColor = new Color(1.0f, 0.5f, 1.0f);
    public float Sensitivity = 2f;

    private float[] SpectrumData = new float[64]; // Holds spectrum analysis

    void Start()
    {
        StartCoroutine(FindAudioManager()); // Search for AudioSource dynamically
    }

    IEnumerator FindAudioManager()
    {
        while (MusicSource == null)
        {
            AudioManager Manager = FindObjectOfType<AudioManager>();
            if (Manager != null) 
                MusicSource = Manager.GetComponent<AudioSource>();

            if (MusicSource == null)
            {
                yield return new WaitForSeconds(0.5f); // Retry every 0.5 seconds
            }
        }
    }

    void Update()
    {
        if (MusicSource == null || !MusicSource.isPlaying) return;

        AnalyzeAudio();
        UpdateSkybox();
    }

    void AnalyzeAudio()
    {
        AudioListener.GetSpectrumData(SpectrumData, 0, FFTWindow.Blackman);

        float Sum = 0;
        for (int i = 0; i < SpectrumData.Length; i++)
            Sum += SpectrumData[i];

        float VolumeLevel = Sum * Sensitivity;
        VolumeLevel = Mathf.Clamp01(VolumeLevel); // Ensure it's between 0 and 1

        // Apply smooth reactive gradient only to the lower sky
        Color ReactiveColor = Color.Lerp(LowVolumeColor, HighVolumeColor, VolumeLevel);
        SkyboxMaterial.SetColor("_GradientColor", ReactiveColor);
    }

    void UpdateSkybox()
    {
        RenderSettings.skybox = SkyboxMaterial;
        SkyboxMaterial.SetColor("_GradientColor", SkyboxMaterial.GetColor("_GradientColor")); // Force Unity to recognize the change
    }
}
