using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 10f;             // Base movement speed
    public float acceleration = 15f;      // How fast the car reaches full speed
    public float deceleration = 10f;      // How fast the car slows down
    public float maxTurnAngle = 15f;      // The tilt effect for turning (optional)

    private float moveInput;              // Player input (-1 to 1)
    private float currentSpeed;           // Current horizontal speed
    private Rigidbody2D rb;

    [SerializeField] private CarGame carGame;

    [SerializeField] private AudioClip carCrash;
    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(!canMove) return;    
        // Get left/right input (-1 for left, 1 for right)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Smooth acceleration & deceleration
        if (moveInput != 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveInput * speed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Optional: Tilt the car slightly when turning
        float tiltAngle = -moveInput * maxTurnAngle;
        transform.rotation = Quaternion.Euler(0, 0, tiltAngle);
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = new Vector2(currentSpeed, 0);
    }


    [SerializeField] private LayerMask deathLayer; // Layer to check

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object belongs to the specified layer
        if (((1 << collision.gameObject.layer) & deathLayer) != 0 && carGame.gameStarted)
        {
            AudioManager.Instance.PlaySound(carCrash);
            Debug.Log("Death with: " + collision.gameObject.name);
            carGame.LoseGame();
            canMove = false;
        }
    }

}
