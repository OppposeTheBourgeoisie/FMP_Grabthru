using UnityEngine;
using TMPro;  // TextMeshPro namespace

public class TutorialTrigger : MonoBehaviour
{
    public TextMeshProUGUI TutorialText;
    private bool TutorialShown = false;

    public string TutorialMessage;

    public float TutorialDisplayTime = 5f;

    private void OnTriggerEnter(Collider Other)
    {
        //See if the player has seen the message before
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

        //Hide the text
        Invoke("HideTutorialText", TutorialDisplayTime);
    }

    private void HideTutorialText()
    {
        TutorialText.gameObject.SetActive(false);
    }
}
