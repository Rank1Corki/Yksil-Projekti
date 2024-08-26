using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 3f;
    public float stepHeight = 0.4f;
    public float stepRayForward = 0.3f;
    public float stepRayHeight = 0.25f;
    public int maxHealth = 100;  // Maximum health of the player
    private int currentHealth;   // Current health of the player

    public Transform cameraTransform;

    private Rigidbody rb;
    private float yRot;
    private float xRot;
    public bool isGrounded;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHealth = maxHealth;  // Initialize player's health
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovementInput();
        HandleJump();
    }

    void FixedUpdate()
    {
        isGrounded = CheckIfGrounded();
        HandleStepClimbing();
        ApplyMovement();
    }

    private void HandleMouseLook()
    {
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);

        xRot -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRot = Mathf.Clamp(xRot, -80f, 80f);
        cameraTransform.localEulerAngles = new Vector3(xRot, 0f, 0f);
    }

    private void HandleMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        moveInput = new Vector3(moveX, 0, moveZ).normalized;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        moveInput *= currentSpeed;
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public bool CheckIfGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void HandleStepClimbing()
    {
        // Raycast to detect if there's a step ahead
        RaycastHit hitLower;
        RaycastHit hitUpper;

        Vector3 originLower = transform.position + Vector3.up * stepRayHeight + transform.forward * stepRayForward;
        Vector3 originUpper = transform.position + Vector3.up * (stepRayHeight + stepHeight) + transform.forward * stepRayForward;

        Debug.DrawRay(originLower, transform.forward * 0.3f, Color.green);
        Debug.DrawRay(originUpper, transform.forward * 0.3f, Color.yellow);

        if (Physics.Raycast(originLower, transform.forward, out hitLower, 0.3f) &&
            !Physics.Raycast(originUpper, transform.forward, out hitUpper, 0.3f))
        {
            rb.position += new Vector3(0, stepHeight, 0);
        }
    }

    private void ApplyMovement()
    {
        if (moveInput.magnitude > 0)
        {
            Vector3 moveVelocity = transform.TransformDirection(moveInput);
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }
        else if (isGrounded)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    // TakeDamage method to handle receiving damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // Add logic for when the player dies, like reloading the level or showing a game over screen
    }
}
