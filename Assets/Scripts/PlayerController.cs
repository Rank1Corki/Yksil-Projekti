using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 5.0f;
    public float mouseSensitivity = 2.0f;
    public Transform cameraTransform;

    public float verticalLookRange = 80.0f;
    public float wallCheckDistance = 0.5f;
    public float wallCheckOffset = 0.1f;  // Offset from player center to check for walls

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private float xRotation = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        moveInput = new Vector3(moveX, 0, moveZ).normalized;

        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;
        moveVelocity = transform.TransformDirection(moveInput) * currentSpeed;

        HandleCollision(ref moveVelocity);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookRange, verticalLookRange);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void HandleCollision(ref Vector3 velocity)
    {
        RaycastHit hit;
        Vector3 playerCenter = transform.position;

        // Check for walls in the direction of movement
        if (Physics.Raycast(playerCenter + transform.TransformDirection(Vector3.forward) * wallCheckOffset,
                            velocity.normalized, out hit, wallCheckDistance))
        {
            Vector3 hitNormal = hit.normal;

            // Adjust the velocity to slide along the wall
            velocity = Vector3.ProjectOnPlane(velocity, hitNormal);

            // Smoothly adjust the velocity to prevent jitter
            velocity = Vector3.Lerp(velocity, transform.TransformDirection(moveInput) * speed, 0.5f);
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}