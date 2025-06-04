using UnityEngine;
using TMPro;

public class TimedDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public float openTime = 3f;
    public Vector3 openOffset = new Vector3(0, 5, 0);
    public ItemPickup itemPickup;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;
    private TextMeshPro doorText;
    private float timeRemaining;

    void Start()
    {
        // Initialize closed and open positions
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
        doorText = GetComponentInChildren<TextMeshPro>();
        if (doorText != null)
            doorText.text = "";
    }

    public void OpenDoor()
    {
        // Sees if the door can be opened
        if (!isOpening)
        {
            isOpening = true;
            StopAllCoroutines();
            StartCoroutine(OpenAndClose());
        }
    }

    private System.Collections.IEnumerator OpenAndClose()
    {
        // Open the door with animation
        float time = 0;
        while (time < 0.5f)
        {
            transform.position = Vector3.Lerp(closedPosition, openPosition, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = openPosition;

        // Show countdown text while door is open
        if (doorText != null)
        {
            timeRemaining = openTime;
            doorText.text = Mathf.Ceil(timeRemaining).ToString() + "s";
        }

        while (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            if (doorText != null)
                doorText.text = Mathf.Ceil(timeRemaining).ToString() + "s";
            yield return null;
        }

        // Hide the text and wait before closing
        if (doorText != null)
            doorText.text = "";

        yield return new WaitForSeconds(0.5f);

        // Close the door with animation
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
