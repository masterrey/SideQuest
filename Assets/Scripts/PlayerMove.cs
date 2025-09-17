using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f; // Speed of player movement
    Vector2 movement; // Movement vector
    public float jumpForce = 300f; // Force applied when jumping
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject BoneRoot;
    public float knockbackForce = 200f; // Force applied when taking damage
    public ParticleSystem Magic;

    bool canJump = true; // Flag to check if the player can jump

    public bool canMove = true;

    public bool invincible = false;

    float distanceToGround; // Distance from the player's center to the ground

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
         movement = new Vector2(Input.GetAxis("Horizontal"), 0);
        // Flip the sprite based on movement direction
        if (movement.x > 0)
        {
            if (spriteRenderer)
                spriteRenderer.flipX = false; // Facing right
            if (BoneRoot)
            {
                BoneRoot.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
        }
        else if (movement.x < 0)
        {
            if (spriteRenderer)
                spriteRenderer.flipX = true; // Facing left
            if (BoneRoot)
                BoneRoot.transform.rotation = Quaternion.Euler(0, 180, 0);

        }

        // Allow jumping only when the player is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector2(0, jumpForce)); // Apply jump force
            canJump = false; // Set canJump to false to prevent double jumping
            animator.SetTrigger("Pulando"); // Trigger the Jump animation
        }

        if(Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Atacando"); // Trigger the Attack animation
            Invoke("FireMagic", 0.35f); // Delay the magic firing to sync with the animation
        }
    }

    void FireMagic()
    {
        if (Magic) Magic.Emit(10);
    }
    // FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        // Prevent movement if canMove is false
        if (!canMove) return;

        // Move the player based on input and speed keeping the current vertical velocity
        //Vector3 newMovement = new Vector3(movement.x * speed, rb.linearVelocity.y, 0);
        //rb.linearVelocity = newMovement;

        // Reduz a força aplicada conforme a velocidade aumenta
        float velocityMultiplier = 1 / (1 + Mathf.Abs(rb.linearVelocityX));

        rb.AddForce(new Vector2(movement.x * speed * velocityMultiplier, 0), ForceMode2D.Force);

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

    public bool DamageTaken(Vector3 relativeImpact)
    {
        if(!canMove || invincible) return false; // Ignore damage if already in knockback or invincible

        animator.SetTrigger("Dano"); // Trigger the Damage animation
        canMove = false; // Disable movement temporarily
        StartCoroutine(KnockBack(relativeImpact)); // Start the knockback coroutine
        return true;
    }

    IEnumerator KnockBack(Vector3 relativeImpact)
    {
        yield return new WaitForFixedUpdate(); // Wait for the next physics update

        spriteRenderer.color = Color.red; // Change color to red to indicate damage

        rb.AddForce(relativeImpact.normalized * knockbackForce,ForceMode2D.Impulse); // Apply knockback force
        
        yield return new WaitForSeconds(0.2f); // Wait for the knockback effect to finish

        rb.linearVelocity = new Vector2(0, 0); 
        canMove = true; // Re-enable movement
        spriteRenderer.color = Color.white; // Reset color to white
        StartCoroutine(Invincible(1f)); // Start temporary invincibility for 2 seconds

    }

    IEnumerator Invincible(float time)
    {
        float elapsed = 0f;
        invincible = true;
        while (true)
        {
            elapsed += Time.deltaTime;
            spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.PingPong(elapsed * 10, 1)); // Flicker effect by changing alpha
            
            if (elapsed >= time)
            {
                invincible = false;
                spriteRenderer.color = Color.white; // Reset color to white
                yield break; // Exit the coroutine after the invincibility duration
            }
            yield return new WaitForFixedUpdate(); // Wait for the next physics update
            
        }

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
