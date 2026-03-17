using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(360f, 180f, 0f);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}