using UnityEngine;
using TMPro;

public class FinalTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI finalTimeText;

    private void Start()
    {
        if (Timer.Instance != null)
        {
            float finalTime = Timer.Instance.GetElapsedTime();
            DisplayFinalTime(finalTime);
        }
    }

    private void DisplayFinalTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);

        finalTimeText.text = "Final Time: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
