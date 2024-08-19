using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 5.0f;
    public float mouseSensitivity = 2.0f;
    public Transform cameraTransform;

    public float verticalLookRange = 80.0f;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private float xRotation = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Hide and lock the cursor in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;
        moveInput = transform.right * moveX + transform.forward * moveZ;
        moveVelocity = moveInput * currentSpeed;

        // Handle jumping input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Handle camera rotation with sensitivity
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        // Limit vertical camera rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookRange, verticalLookRange);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
    }

    void FixedUpdate()
    {
        // Check if the player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Apply movement in FixedUpdate for smooth physics integration
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    // Public method to get the grounded status
    public bool IsGrounded()
    {
        return isGrounded;
    }
}
