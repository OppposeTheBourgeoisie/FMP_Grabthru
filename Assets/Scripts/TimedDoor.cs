using UnityEngine;
using TMPro;  // Make sure you have TextMeshPro package

public class TimedDoor : MonoBehaviour
{
    public float openTime = 3f; // Time door stays open
    public Vector3 openOffset = new Vector3(0, 5, 0); // How far the door moves when opened
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;

    public ItemPickup itemPickup;

    // Reference to TextMeshPro attached to the door
    private TextMeshPro doorText;
    private float timeRemaining;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;

        // Find and assign the TextMeshPro component attached to the door
        doorText = GetComponentInChildren<TextMeshPro>();

        // Initialize the text to be empty initially
        if (doorText != null)
        {
            doorText.text = "";
        }
    }

    public void OpenDoor()
    {
        if (!isOpening)
        {
            isOpening = true;
            StopAllCoroutines();
            StartCoroutine(OpenAndClose());
        }
    }

    private System.Collections.IEnumerator OpenAndClose()
    {
        // Start opening the door
        float time = 0;
        while (time < 0.5f)
        {
            transform.position = Vector3.Lerp(closedPosition, openPosition, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = openPosition;

        // Show the remaining time text and set the countdown
        if (doorText != null)
        {
            timeRemaining = openTime;
            doorText.text = Mathf.Ceil(timeRemaining).ToString() + "s";  // Show initial time remaining
        }

        // Countdown while the door is open
        while (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            doorText.text = Mathf.Ceil(timeRemaining).ToString() + "s";  // Update text with remaining time
            yield return null;
        }

        // Hide the text once the door closes
        if (doorText != null)
        {
            doorText.text = "";
        }

        // Wait before closing the door
        yield return new WaitForSeconds(0.5f);

        // Start closing the door
        time = 0;
        while (time < 0.5f)
        {
            transform.position = Vector3.Lerp(openPosition, closedPosition, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = closedPosition;

        isOpening = false;
    }
}
