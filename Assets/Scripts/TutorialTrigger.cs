using UnityEngine;
using TMPro;

public class TutorialTrigger : MonoBehaviour
{
    public TextMeshProUGUI TutorialText;
    private bool TutorialShown = false;

    public string TutorialMessage;

    public float TutorialDisplayTime = 5f;

    private void OnTriggerEnter(Collider Other)
    {
        //See if the player has seen the message before and shows the tutorial text if not
        if (Other.CompareTag("Player") && !TutorialShown)
        {
            ShowTutorialText();
        }
    }

    private void ShowTutorialText()
    {
        //Show the tutorial message
        if (!string.IsNullOrEmpty(TutorialMessage))
        {
            TutorialText.gameObject.SetActive(true);
            TutorialText.text = TutorialMessage;
        }

        TutorialShown = true;
        Invoke("HideTutorialText", TutorialDisplayTime);
    }

    private void HideTutorialText()
    {
        // Hide the tutorial message after a delay
        TutorialText.gameObject.SetActive(false);
    }
}
