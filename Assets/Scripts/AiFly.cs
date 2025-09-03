using UnityEngine;

public class AiFly : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public float speed = 5f; // Speed of the AI movement
    
    
    SpriteRenderer spriteRenderer;

    Rigidbody2D rb;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    // Update is called once per frame
    void Update()
    {
        

    }
    private void FixedUpdate()
    {
        //check if player is null to avoid errors
        if (player == null) return;

        Vector3 res = player.transform.position - transform.position;
        res.Normalize();

        //if ternary to flip the sprite based on movement direction
        spriteRenderer.flipX = res.x > 0 ? true : false;

        rb.linearVelocity = res * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
            rb.linearVelocity = Vector2.zero; // Stop movement when player exits the trigger
        }
    }

}
