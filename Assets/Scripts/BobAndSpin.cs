using UnityEngine;

public class BobAndSpin : MonoBehaviour
{
    public float bobHeight = 0.5f;  // Height of the bobbing motion
    public float bobSpeed = 2.0f;   // Speed of the bobbing motion
    public float spinSpeed = 100.0f; // Speed of the spinning motion

    private Vector3 startPosition;  // Original position of the object

    void Start()
    {
        // Save the original position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Bobbing motion: Adjust the Y position over time using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        // Apply the new position
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Spinning motion: Rotate the object around its Y-axis
        transform.Rotate(Vector3.left, spinSpeed * Time.deltaTime);
    }
}
