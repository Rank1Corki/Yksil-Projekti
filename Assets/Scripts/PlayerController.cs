using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 3f;
    public float stepHeight = 0.3f;
    public float stepRayUpper = 0.45f;
    public float stepRayLower = 0.1f;
    public float wallSlideFriction = 0.5f; // Friction factor when sliding along walls

    public Transform cameraTransform;

    private Rigidbody rb;
    private float yRot;
    private float xRot;
    private bool isGrounded;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle mouse rotation
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);

        xRot -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRot = Mathf.Clamp(xRot, -80f, 80f); // Limit vertical look to avoid flipping
        cameraTransform.localEulerAngles = new Vector3(xRot, 0f, 0f);

        // Handle movement input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        moveInput = new Vector3(moveX, 0, moveZ).normalized;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        moveVelocity = transform.TransformDirection(moveInput) * currentSpeed;

        // Jumping logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (moveInput.magnitude > 0)
        {
            StepClimb();  // Handle climbing small steps like stairs
            HandleWallSliding();  // Handle sliding along walls

            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }
        else if (isGrounded)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void StepClimb()
    {
        // Raycast to detect if there's a step ahead
        RaycastHit hitLower;
        RaycastHit hitUpper;

        Vector3 originLower = transform.position + Vector3.up * stepRayLower;
        Vector3 originUpper = transform.position + Vector3.up * stepRayUpper;

        if (Physics.Raycast(originLower, transform.forward, out hitLower, 0.3f) &&
            !Physics.Raycast(originUpper, transform.forward, out hitUpper, 0.3f))
        {
            rb.position += new Vector3(0, stepHeight, 0);
        }
    }

    private void HandleWallSliding()
    {
        RaycastHit hit;
        Vector3 movementDirection = moveVelocity.normalized;

        // Cast a ray in the direction of movement
        if (Physics.Raycast(transform.position, movementDirection, out hit, 0.5f))
        {
            Vector3 hitNormal = hit.normal;

            // Adjust the movement direction to slide along the wall
            Vector3 slideDirection = Vector3.ProjectOnPlane(movementDirection, hitNormal);
            moveVelocity = slideDirection * moveVelocity.magnitude * wallSlideFriction;
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
