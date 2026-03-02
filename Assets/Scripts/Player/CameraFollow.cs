using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    public float yaw = 0f;
    public float pitch = 30f;
    public float distance = 10f;
    public float sensitivity = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, 10f, 80f);
    }

    private void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = playerTransform.position + offset;
        transform.LookAt(playerTransform.position);
    }
}
