using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed = 10f; // Speed of camera movement
    public GameObject target; // Target object to follow
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the camera towards the target object
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
            transform.position = newPosition;
           
        }

    }
}
