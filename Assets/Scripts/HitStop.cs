using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void DoHitStop(float duration, float timeScale = 0f)
    {
        // Start the hit stop effect for a set duration and timescale
        StartCoroutine(HitStopCoroutine(duration, timeScale));
    }

    private IEnumerator HitStopCoroutine(float duration, float timeScale)
    {
        // Change timescale and fixedDeltaTime for the duration of the hit stop
        float originalTimeScale = Time.timeScale;
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
