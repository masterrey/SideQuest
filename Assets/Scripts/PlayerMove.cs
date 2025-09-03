using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f; // Speed of player movement
    Vector2 movement; // Movement vector
    public float jumpForce = 300f; // Force applied when jumping
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float knockbackForce = 200f; // Force applied when taking damage

    bool canJump = true; // Flag to check if the player can jump

    bool canMove = true;

    float distanceToGround; // Distance from the player's center to the ground

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
         movement = new Vector2(Input.GetAxis("Horizontal"), 0);
        // Flip the sprite based on movement direction
        if (movement.x > 0)
            spriteRenderer.flipX = false; // Facing right
        else if (movement.x < 0)
            spriteRenderer.flipX = true; // Facing left

        // Allow jumping only when the player is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector2(0, jumpForce)); // Apply jump force
            canJump = false; // Set canJump to false to prevent double jumping
            animator.SetTrigger("Pulando"); // Trigger the Jump animation
        }
    }
    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        // Prevent movement if canMove is false
        if (!canMove) return;

        // Move the player based on input and speed keeping the current vertical velocity
        Vector3 newMovement = new Vector3(movement.x * speed, rb.linearVelocity.y, 0);
        rb.linearVelocity = newMovement;
        animator.SetFloat("Velocidade", Mathf.Abs(movement.x)); // Update the Speed parameter in the Animator

        // Raycast downwards to check the distance to the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 20f);
        if (hit.collider != null)
        {
            distanceToGround = hit.distance;
            animator.SetFloat("DistanciaChao", distanceToGround); // Update the DistanceToGround parameter in the Animator
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
    }

    public void DamageTaken(Vector3 relativeImpact)
    {
        animator.SetTrigger("Dano"); // Trigger the Damage animation
        canMove = false; // Disable movement temporarily
        StartCoroutine(KnockBack(relativeImpact)); // Start the knockback coroutine
    }

    IEnumerator KnockBack(Vector3 relativeImpact)
    {
        yield return new WaitForFixedUpdate(); // Wait for the next physics update

        rb.AddForce(relativeImpact.normalized * knockbackForce,ForceMode2D.Impulse); // Apply knockback force
        
        yield return new WaitForSeconds(0.2f); // Wait for the knockback effect to finish

        rb.linearVelocity = new Vector2(0, 0); 
        canMove = true; // Re-enable movement
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true; // Allow jumping again when the player is on the ground
        }
    }

    

}
