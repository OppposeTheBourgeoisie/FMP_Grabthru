using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public TextMeshProUGUI cutsceneText;
    public float fadeDuration = 1f;
    public float holdDuration = 2f;
    public string nextSceneName = "MainLevel";

    private void Start()
    {
        cutsceneText.alpha = 0f; // Start fully transparent
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // Fade in
        yield return StartCoroutine(FadeText(0f, 1f, fadeDuration));

        // Hold
        yield return new WaitForSeconds(holdDuration);

        // Fade out
        yield return StartCoroutine(FadeText(1f, 0f, fadeDuration));

        // Load next scene
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeText(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            cutsceneText.alpha = Mathf.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cutsceneText.alpha = to;
    }
}
