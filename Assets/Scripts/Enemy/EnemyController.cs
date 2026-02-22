using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform playerTransform;
    public float speed;
    Rigidbody rb;
    Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3(playerTransform.position.x - this.transform.position.x,
            0, playerTransform.position.z - this.transform.position.z).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }
}
