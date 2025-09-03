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

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerMove.DamageTaken(collision.contacts[0].relativeVelocity);

            health -= 10;
            Debug.Log("Player Health: " + health);
        }
    }
}
