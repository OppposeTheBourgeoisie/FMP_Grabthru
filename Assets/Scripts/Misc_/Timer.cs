using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    private float ElapsedTime = 0f;
    private bool IsRunning = true;

    public static Timer Instance;

    private void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (IsRunning)
        {
            ElapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        //Format the timer
        int Minutes = Mathf.FloorToInt(ElapsedTime / 60);
        int Seconds = Mathf.FloorToInt(ElapsedTime % 60);
        int Milliseconds = Mathf.FloorToInt((ElapsedTime * 100) % 100);

        TimerText.text = string.Format("{0:00}:{1:00}:{2:00}", Minutes, Seconds, Milliseconds);
    }

    public void StopTimer()
    {
        //Stop the timer when the level ends
        IsRunning = false;
        
        if (TimerText != null)
        {
            TimerText.gameObject.SetActive(false);
        }
    }

    public float GetElapsedTime()
    {
        return ElapsedTime;
    }
}
