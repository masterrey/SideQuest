using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    PlayerMove playerMove;

    public int health = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (playerMove.DamageTaken(transform.position - collision.transform.position))
            {
                health -= 10;
                Debug.Log("Player Health: " + health);
            }
            if (health <= 0)
            {
                StartCoroutine(Die(collision.otherCollider));
            }
        }
    }

    IEnumerator Die(Collider2D collider)
    {
        yield return new WaitForSeconds(1f);
        playerMove.animator.SetBool("Morto", true);
        playerMove.canMove = false;
        collider.enabled = false; // Disable enemy collider to prevent further collisions
        playerMove.rb.AddForce(new Vector2(0, playerMove.knockbackForce), ForceMode2D.Impulse); // Apply upward force on death
        Destroy(gameObject, 3f); // Destroy the player object after 1 second

    }
}
