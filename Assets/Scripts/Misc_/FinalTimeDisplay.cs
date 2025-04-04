using UnityEngine;
using TMPro;

public class FinalTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI FinalTimeText;

    private void Start()
    {
        //Stop the timer when the level ends
        if (Timer.Instance != null)
        {
            float finalTime = Timer.Instance.GetElapsedTime();
            DisplayFinalTime(finalTime);
        }
    }

    private void DisplayFinalTime(float time)
    {
        //Format for the timer
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);

        FinalTimeText.text = "Final Time: " + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
