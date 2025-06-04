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
        // Start with text invisible and begin cutscene sequence
        cutsceneText.alpha = 0f;
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // Fade in, hold, fade out, then load next scene
        yield return StartCoroutine(FadeText(0f, 1f, fadeDuration));
        yield return new WaitForSeconds(holdDuration);
        yield return StartCoroutine(FadeText(1f, 0f, fadeDuration));
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator FadeText(float from, float to, float duration)
    {
        // Lerp the alpha of the text over time
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
