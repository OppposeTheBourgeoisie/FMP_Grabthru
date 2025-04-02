using UnityEngine;

public class CreateGrappleable : MonoBehaviour
{
    // Boolean to track whether the object has been picked up
    public bool OtherPickup;

    public ItemPickup itemPickup;

    // Define the target Z position the object should move to
    public float targetZPosition = 10f; // Change this to your desired Z position

    private void Update()
    {
        if (itemPickup.OtherPickup)
        {
            ActivateItem();
        }
    }

    void ActivateItem()
    {
        // Move the object to the desired location on the Z-axis and keep it there
        transform.position = new Vector3(transform.position.x, transform.position.y, targetZPosition);
    }
}
