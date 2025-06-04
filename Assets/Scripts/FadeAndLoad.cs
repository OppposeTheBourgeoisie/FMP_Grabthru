using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FadeAndLoad : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public string sceneToLoad = "CutsceneScene";

    private bool hasFaded = false;

    private void OnTriggerEnter(Collider other)
    {
        // Load the scene and begin screen fade
        if (!hasFaded && other.CompareTag("Player"))
        {
            hasFaded = true;
            StartCoroutine(FadeOutAndLoad());
        }
    }

    private IEnumerator FadeOutAndLoad()
    {
        // Fade out the image and load the next scene
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            color.a = Mathf.Lerp(0f, 1f, t);
            fadeImage.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        SceneManager.LoadScene(sceneToLoad);
    }
}
