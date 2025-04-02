using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    float InteractDist = 2f;
    bool Interacting = false;

    // Start is called before the first frame update
    void Start()
    {
        Interacting = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] ColliderArray = Physics.OverlapSphere(transform.position, InteractDist);
            foreach (Collider collider in ColliderArray)
            {
                // Check for ItemPickup component in the collider
                if (collider.TryGetComponent(out ItemPickup itemPickup))
                {
                    // Check the tag of the collider and interact with it
                    if (!Interacting)
                    {
                        if (collider.CompareTag("Match"))
                        {
                            Interacting = true;
                            itemPickup.MatchPickedUp();  // Call the method to mark the item as picked up
                        }
                        else if (collider.CompareTag("Other"))
                        {
                            Interacting = true;
                            itemPickup.OtherPickedUp();  // Call the method to mark the item as picked up
                        }
                    }
                }
            }
        }

        // Reset interaction if the player moves out of range (optional)
        if (Vector3.Distance(transform.position, Camera.main.transform.position) > InteractDist)
        {
            Interacting = false;
        }
    }

    // Optionally, you can also use this function to get the item that's interactable
    public ItemPickup GetInteractableObject()
    {
        Collider[] ColliderArray = Physics.OverlapSphere(transform.position, InteractDist);
        foreach (Collider collider in ColliderArray)
        {
            if (collider.TryGetComponent(out ItemPickup itemPickup))
            {
                return itemPickup;
            }
        }

        return null;
    }
}
