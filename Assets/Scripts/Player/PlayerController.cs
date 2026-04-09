using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 7f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public float jumpforce = 5.0f;

    public LayerMask groundLayer;
    private float lastJumpTime;
    public float jumpCooldown = 2f;

    Rigidbody rb;
    Vector3 direction;
    private Quaternion targetRotation;

    private Animator animator;
    public Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;

        targetRotation = transform.rotation;
    }

    void Update()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        direction = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");
        direction.Normalize();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastJumpTime + jumpCooldown && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        }

        if (direction.magnitude > 0.1f)
        {
            targetRotation = Quaternion.LookRotation(direction);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 800f * Time.deltaTime);

        float angleToTarget = Vector3.Angle(transform.forward, direction);
        float currentSpeed = (direction.magnitude > 0.1f && angleToTarget < 60f) ? 1f : 0f;
        animator.SetFloat("Speed", currentSpeed);
    }

    private void FixedUpdate()
    {
        float currentYVelocity = rb.linearVelocity.y;
        Vector3 newVelocity = Vector3.zero;

        if (direction.magnitude > 0.1f)
        {
            float angle = Vector3.Angle(transform.forward, direction);

            if (angle < 60f)
            {
                newVelocity = direction * speed;
            }
        }

        newVelocity.y = currentYVelocity;
        rb.linearVelocity = newVelocity;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.01f, groundLayer);
    }

    public void AddSpeed(float speed)
    {
        this.speed += speed;
    }
}