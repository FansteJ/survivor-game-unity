using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 8, playerTransform.position.z - 10);
    }
}
