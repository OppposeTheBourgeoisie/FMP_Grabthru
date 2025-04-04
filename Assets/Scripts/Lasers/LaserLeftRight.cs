using UnityEngine;

public class LaserLeftRight : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float MoveRange = 10f;
    private Vector3 InitialPosition;

    void Start()
    {
        InitialPosition = transform.position;
    }

    void Update()
    {
        //Move the laser from left to right and back
        float NewX = Mathf.Sin(Time.time * MoveSpeed) * MoveRange;
        transform.position = new Vector3(InitialPosition.x + NewX, transform.position.y, transform.position.z);
    }
}
