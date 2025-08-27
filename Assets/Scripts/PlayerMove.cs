using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f; // Speed of player movement
    Vector2 movement; // Movement vector
    public float jumpForce = 300f; // Force applied when jumping
    Rigidbody2D rb;
    Animator animator;

    bool canJump = true; // Flag to check if the player can jump

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         movement = new Vector2(Input.GetAxis("Horizontal"), 0);

        // Allow jumping only when the player is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector2(0, jumpForce)); // Apply jump force
            canJump = false; // Set canJump to false to prevent double jumping
        }

    }
    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        // Move the player based on input and speed keeping the current vertical velocity
        Vector3 newMovement = new Vector3(movement.x * speed, rb.linearVelocity.y, 0);
        rb.linearVelocity = newMovement;
        animator.SetFloat("Velocidade", Mathf.Abs(movement.x)); // Update the Speed parameter in the Animator
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collides with the ground or any other object
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destroy the player object if it collides with an enemy

        }

        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true; // Allow jumping again when the player is on the ground
        }
    }

}
