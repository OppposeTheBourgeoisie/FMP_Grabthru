using UnityEngine;
using TMPro;

public class TimedDoor : MonoBehaviour
{
    public float OpenTime = 3f;
    public Vector3 OpenOffset = new Vector3(0, 5, 0);
    private Vector3 ClosedPosition;
    private Vector3 OpenPosition;
    private bool IsOpening = false;

    public ItemPickup ItemPickup;

    private TextMeshPro DoorText;
    private float TimeRemaining;

    void Start()
    {
        ClosedPosition = transform.position;
        OpenPosition = ClosedPosition + OpenOffset;

        DoorText = GetComponentInChildren<TextMeshPro>(); 

        if (DoorText != null)
        {
            // While the door is closed, hide the text
            DoorText.text = "";
        }
    }

    public void OpenDoor()
    {
        // Open the door and start the countdown
        if (!IsOpening)
        {
            IsOpening = true;
            StopAllCoroutines();
            StartCoroutine(OpenAndClose());
        }
    }

    private System.Collections.IEnumerator OpenAndClose()
    {
        // Open the door
        float elapsedTime = 0;
        while (elapsedTime < 0.5f)
        {
            transform.position = Vector3.Lerp(ClosedPosition, OpenPosition, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = OpenPosition;

        // Show the remaining time text and set the countdown
        if (DoorText != null)
        {
            TimeRemaining = OpenTime;
            DoorText.text = Mathf.Ceil(TimeRemaining).ToString() + "s";
        }

        // When the time runs out, close the door
        while (TimeRemaining > 0f)
        {
            TimeRemaining -= Time.deltaTime;
            DoorText.text = Mathf.Ceil(TimeRemaining).ToString() + "s";
            yield return null;
        }

        if (DoorText != null)
        {
            DoorText.text = "";
        }

        yield return new WaitForSeconds(0.5f);

        // Start closing the door
        elapsedTime = 0;
        while (elapsedTime < 0.5f)
        {
            transform.position = Vector3.Lerp(OpenPosition, ClosedPosition, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = ClosedPosition;

        IsOpening = false;
    }
}
