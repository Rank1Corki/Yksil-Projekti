using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    public float bobbingAmount = 0.1f; // Amount of bobbing
    public float bobbingSpeed = 2.0f;  // Speed of bobbing
    public float sprintBobbingSpeedMultiplier = 2.0f; // Multiplier for bobbing speed while sprinting
    public float smoothingSpeed = 0.1f; // Smoothing speed

    private float defaultY;
    private float timer = 0.0f;
    private Vector3 originalPosition;

    private PlayerController playerController; // Reference to the player controller script

    void Start()
    {
        // Store the original position of the camera
        originalPosition = transform.localPosition;
        defaultY = transform.localPosition.y;
        playerController = GetComponentInParent<PlayerController>(); // Get the PlayerController component
    }

    void Update()
    {
        if (playerController != null)
        {
            // Get the movement input
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Check if the player is moving
            bool isSprinting = Input.GetKey(KeyCode.LeftShift); // Check if the sprint key is pressed
            bool isGrounded = playerController.CheckIfGrounded(); // Check if the player is on the ground
            float currentBobbingSpeed = isSprinting ? bobbingSpeed * sprintBobbingSpeedMultiplier : bobbingSpeed;

            if (isGrounded && (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f))
            {
                // Bobbing effect
                timer += Time.deltaTime * currentBobbingSpeed;
                float y = Mathf.Sin(timer) * bobbingAmount;
                transform.localPosition = new Vector3(originalPosition.x, defaultY + y, originalPosition.z);
            }
            else
            {
                // Smoothly return to the original position when not moving or in the air
                timer = 0;
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, smoothingSpeed);
            }
        }
    }
}
