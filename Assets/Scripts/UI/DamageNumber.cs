using TMPro;
using UnityEngine;


public class DamageNumber : MonoBehaviour
{
    public float speed = 1f;
    public float fadeSpeed = 1f;

    public TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        text.alpha -= Time.deltaTime * fadeSpeed;
        if (text.alpha <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
