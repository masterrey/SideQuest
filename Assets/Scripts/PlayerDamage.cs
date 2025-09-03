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
        }
    }
}
