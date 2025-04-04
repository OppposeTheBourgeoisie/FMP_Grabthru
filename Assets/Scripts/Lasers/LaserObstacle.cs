using UnityEngine;

public class LaserObstacle : MonoBehaviour
{
    private GameObject RespawnLocation;

    private void OnTriggerEnter(Collider other)
    {
        //Check if the player collides with the laser
        if (other.CompareTag("Player"))
        {
            FindRespawnLocation();

            if (RespawnLocation != null)
            {
                TeleportPlayer(other.gameObject);
            }
            else
            {
                Debug.LogError("No RespawnLocation found in the scene!");
            }
        }
    }

    void FindRespawnLocation()
    {
        //Find the respawn location
        RespawnLocation = GameObject.FindWithTag("RespawnPoint"); 
    }

    void TeleportPlayer(GameObject player)
    {
        //Teleports the player to the respawn location
        player.transform.position = RespawnLocation.transform.position;
    }
}
