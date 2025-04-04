using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    float InteractDist = 2f;
    bool Interacting = false;

    void Start()
    {
        Interacting = false;
    }

    private void Update()
    {
        //Check if the player is pressing the interact key (E) and if the object is not already being interacted with
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] ColliderArray = Physics.OverlapSphere(transform.position, InteractDist);
            foreach (Collider collider in ColliderArray)
            {
                if (collider.TryGetComponent(out ItemPickup itemPickup))
                {
                    if (!Interacting && CompareTag("Match"))
                    {
                        Interacting = true;
                        itemPickup.MatchPickedUp();
                    }

                    if (!Interacting && CompareTag("Other"))
                    {
                        Interacting = true;
                        itemPickup.OtherPickedUp();
                    }
                }
            }
        }
    }

    public ItemPickup GetInteractableObject()
    {
        //Check what the player interacted with and return that object
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
