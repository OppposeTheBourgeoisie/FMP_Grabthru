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
