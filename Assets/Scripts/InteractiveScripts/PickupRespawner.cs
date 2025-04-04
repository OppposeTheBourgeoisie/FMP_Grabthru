using System.Collections;
using UnityEngine;

public class PickupRespawner : MonoBehaviour
{
    public float RespawnTime = 5f;

    private Collider PickupCollider;
    private Renderer[] renderers;

    private void Start()
    {
        PickupCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void StartRespawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        //Disable the object
        foreach (var r in renderers) r.enabled = false;

        PickupCollider.enabled = false;

        yield return new WaitForSeconds(RespawnTime);

        //Enable the object
        foreach (var r in renderers) r.enabled = true;

        PickupCollider.enabled = true;
    }
}
