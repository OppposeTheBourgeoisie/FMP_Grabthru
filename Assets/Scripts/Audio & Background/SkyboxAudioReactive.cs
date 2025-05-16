using UnityEngine;
using System.Collections;

public class SkyboxAudioReactive : MonoBehaviour
{
    private AudioSource musicSource;
    public Material skyboxMaterial; // Assign the Skybox Material
    public Color lowVolumeColor = new Color(0.2f, 0.0f, 0.5f); // Darker tone
    public Color highVolumeColor = new Color(1.0f, 0.5f, 1.0f); // Brighter reactive tone
    public float sensitivity = 2f; // Lowered for smoother transitions

    private float[] spectrumData = new float[64]; // Holds spectrum analysis

    void Start()
    {
        StartCoroutine(FindAudioManager()); // Search for AudioSource dynamically
    }

    IEnumerator FindAudioManager()
    {
        while (musicSource == null)
        {
            AudioManager manager = FindObjectOfType<AudioManager>();
            if (manager != null) 
                musicSource = manager.GetComponent<AudioSource>();

            if (musicSource == null)
            {
                yield return new WaitForSeconds(0.5f); // Retry every 0.5 seconds
            }
        }
    }

    void Update()
    {
        if (musicSource == null || !musicSource.isPlaying) return;

        AnalyzeAudio();
        UpdateSkybox();
    }

    void AnalyzeAudio()
    {
        AudioListener.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        float sum = 0;
        for (int i = 0; i < spectrumData.Length; i++)
            sum += spectrumData[i];

        float volumeLevel = sum * sensitivity;
        volumeLevel = Mathf.Clamp01(volumeLevel); // Ensure it's between 0 and 1

        // Apply smooth reactive gradient only to the lower sky
        Color reactiveColor = Color.Lerp(lowVolumeColor, highVolumeColor, volumeLevel);
        skyboxMaterial.SetColor("_GradientColor", reactiveColor);
    }

    void UpdateSkybox()
    {
        RenderSettings.skybox = skyboxMaterial;
        skyboxMaterial.SetColor("_GradientColor", skyboxMaterial.GetColor("_GradientColor")); // Force Unity to recognize the change
    }
}
