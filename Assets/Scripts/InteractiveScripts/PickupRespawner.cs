using System.Collections;
using UnityEngine;

public class PickupRespawner : MonoBehaviour
{
    public float RespawnTime = 5f;

    private Collider PickupCollider;
    private Renderer[] renderers;

    private void Start()
    {
        // Initialize the collider and renderers
        PickupCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void StartRespawn()
    {
        // Start the respawn coroutine
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        // When the pickup is collected, disable the renderers and collider
        foreach (var r in renderers) r.enabled = false;
        PickupCollider.enabled = false;

        yield return new WaitForSeconds(RespawnTime);

        foreach (var r in renderers) r.enabled = true;
        PickupCollider.enabled = true;
    }
}
