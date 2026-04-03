using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 7f;
    public float jumpforce = 5.0f;

    public LayerMask groundLayer;
    private float lastJumpTime;
    public float jumpCooldown = 2f;

    Rigidbody rb;
    Vector3 direction;

    private Animator animator;

    public Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        direction = forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        }
        animator.SetFloat("Speed", direction.magnitude);
        if(direction.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        float currentYVelocity = rb.linearVelocity.y;

        Vector3 newVelocity = direction * speed;
        newVelocity.y = currentYVelocity;

        rb.linearVelocity = newVelocity;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.01f, groundLayer);
    }
}
