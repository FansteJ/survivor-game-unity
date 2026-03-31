using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 1f;
    public float fadeSpeed = 1f;
    public float scatterAmount = 1.5f;

    [Header("References")]
    public TMP_Text text;
    public GameObject prefab;

    private Vector3 moveDirection;

    private void OnEnable()
    {
        text.alpha = 1f;

        float randomX = Random.Range(-scatterAmount, scatterAmount);
        float randomZ = Random.Range(-scatterAmount, scatterAmount);

        moveDirection = new Vector3(randomX, speed, randomZ);
    }

    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        moveDirection.x = Mathf.Lerp(moveDirection.x, 0, Time.deltaTime * 3f);
        moveDirection.z = Mathf.Lerp(moveDirection.z, 0, Time.deltaTime * 3f);

        text.alpha -= Time.deltaTime * fadeSpeed;
        if (text.alpha <= 0f)
        {
            PoolManager.Instance.Return(prefab, gameObject);
        }
    }
}