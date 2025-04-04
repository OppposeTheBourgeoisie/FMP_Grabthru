using UnityEngine;

public class LaserUpDown : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public float MoveRange = 5f;
    private Vector3 InitialPosition;

    void Start()
    {
        InitialPosition = transform.position;
    }

    void Update()
    {
        //Move the laser up and down and back
        float newY = Mathf.Sin(Time.time * MoveSpeed) * MoveRange;
        transform.position = new Vector3(transform.position.x, InitialPosition.y + newY, transform.position.z);
    }
}
