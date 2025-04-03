using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundVisualizer : MonoBehaviour
{
    private AudioSource audioSource;
    public Image backgroundImage; // 🎨 Assign your UI Image here
    public float colorIntensity = 5f;

    private float[] spectrumData = new float[64];

    void Start()
    {
        StartCoroutine(FindAudioManager());
    }

    IEnumerator FindAudioManager()
    {
        while (audioSource == null)
        {
            AudioManager manager = FindObjectOfType<AudioManager>();
            if (manager != null) audioSource = manager.GetComponent<AudioSource>();

            if (audioSource == null)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void Update()
    {
        if (audioSource == null || backgroundImage == null) return;

        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);
        float intensity = spectrumData[0] * colorIntensity;

        // 🔹 Background Color Flashing Effect
        Color baseColor = new Color(0.2f, 0.0f, 0.5f);
        Color flashColor = new Color(1f, 0.5f, 1f);
        Color newColor = Color.Lerp(baseColor, flashColor, intensity);
        
        backgroundImage.color = newColor; // 🎨 Apply color to UI Image
    }
}
