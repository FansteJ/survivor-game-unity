using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float speed = 200f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -speed *  Time.deltaTime);
    }
}
