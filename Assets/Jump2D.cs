using UnityEngine;

public class Jump2D : MonoBehaviour
{
    public float jumpForce = 5f;            // The base force applied for the jump
    public float maxJumpHeight = 10f;       // The maximum height for the jump
    public float jumpHoldMultiplier = 2f;   // How much force increases with holding the jump button
    public LayerMask groundLayer;           // To check if the object is grounded
    private Rigidbody2D rb;                 // Reference to the Rigidbody2D component
    private bool isGrounded;                // Flag to check if the object is grounded
    private float jumpTime = 0f;            // Timer for how long the jump button has been held down
    [SerializeField] private float checkFloorDistance = 0.2f;

    [SerializeField] private float maxHoldTime = 1f; // Maximum time the jump can be held

    [SerializeField] private Transform footObject;

    [SerializeField] private float gravityScaleWhileHolding;
    private float originalGravityScale;

    private bool isJumping = false;

    [SerializeField] private ChaseDogGame chaseDogGame;

    [SerializeField] private HopAnimation HopAnimation;
    [SerializeField] private Animator animator;

    public bool canJump = false;

    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    public bool ToggleAnimator(bool toggle) => animator.enabled = toggle;

    void Update()
    {
        rb.linearVelocityX = 0;
        if (!canJump)
            return;
        
        // Check if the object is grounded and the player presses the jump button (spacebar)
        if (IsGrounded())
        {
            HopAnimation.StartHopping();
            ToggleAnimator(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HopAnimation.StopHopping();
                ToggleAnimator(false);
                isJumping = true;
                // If the jump button is pressed down, make the jump start
                jumpTime = 0f;  // Reset jump time when first pressed
                Jump(jumpForce);
            }
        }

        // If the spacebar is pressed, start holding the jump
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            // Start counting how long the jump button is held
            jumpTime += Time.deltaTime;

            rb.gravityScale = gravityScaleWhileHolding;
        }

        // If the spacebar is released, stop applying any extra jump force
        if (Input.GetKeyUp(KeyCode.Space) || jumpTime > maxHoldTime)
        {
            rb.gravityScale = originalGravityScale;
            isJumping = false;
            jumpTime = 0f; // Reset timer when released
        }
    }

    // Method to apply the jump force
    void Jump(float currentJumpForce)
    {
        // Reset the Y velocity before applying the jump
        rb.linearVelocity = new Vector2(0, 0); // You can also reset the entire velocity if needed

        // Apply the calculated force as an impulse
        rb.AddForce(Vector2.up * currentJumpForce, ForceMode2D.Impulse);
    }

    // Method to check if the object is on the ground
    bool IsGrounded()
    {
        // Cast a small ray downwards to check if the object is touching the ground
        RaycastHit2D hit = Physics2D.Raycast(footObject.transform.position, Vector2.down, checkFloorDistance, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        // Draw a small ray to visualize the ground check
        Gizmos.color = Color.red;
        Gizmos.DrawLine(footObject.transform.position, footObject.transform.position + Vector3.down * checkFloorDistance);
    }

    [SerializeField] private LayerMask deathLayer; // Layer to check
    [SerializeField] private LayerMask dogLayer; // Layer to check

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object belongs to the specified layer
        if (((1 << other.gameObject.layer) & deathLayer) != 0)
        {
            Debug.Log("Death with: " + other.gameObject.name);
            chaseDogGame.LoseGame();
        }

        // Check if the object belongs to the specified layer
        if (((1 << other.gameObject.layer) & dogLayer) != 0)
        {
            Debug.Log("Cat with: " + other.gameObject.name);
            chaseDogGame.WinGame();
        }
    }
}
