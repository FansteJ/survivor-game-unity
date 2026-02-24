using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public Transform playerTransform;
    public float speed;
    Rigidbody rb;
    Vector3 direction;

    public float damage = 10f;
    public float damageCooldown = 1f;
    private float lastDamageTime;
    public float knockbackForce = 20f;

    public float stopDistance = 0.25f;

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
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        float currentYVelocity = rb.linearVelocity.y;

        if (distance > stopDistance)
        {
            Vector3 newVelocity = direction * speed;
            newVelocity.y = currentYVelocity;
            rb.linearVelocity = newVelocity;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, currentYVelocity, 0);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && Time.time > lastDamageTime + damageCooldown)
        {
            lastDamageTime = Time.time;
            Vector3 knockbackDir = (collision.transform.position - transform.position).normalized;
            knockbackDir.y = 0;
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, knockbackDir, knockbackForce);
        }
    }
}
