using System.Collections;
using UnityEngine;

public class XpOrb : MonoBehaviour
{
    private Transform player;

    public float xpAmount = 10f;
    public GameObject prefab;

    private float currentSpeed = 0f;
    public float acceleration = 10f;

    public float popDuration = 0.3f;
    public float popRadius = 1.5f;
    public float targetYOffset = 1.2f;

    private bool canFly = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (canFly && player != null)
        {
            currentSpeed += acceleration * Time.deltaTime;

            Vector3 targetPosition = player.position + (Vector3.up * targetYOffset);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        currentSpeed = 0f;
        canFly = false;
        StartCoroutine(PopAndFlyRoutine());
    }

    IEnumerator PopAndFlyRoutine()
    {
        Vector3 startPos = transform.position;

        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1.5f, Random.Range(-1f, 1f)).normalized;

        Vector3 targetPos = startPos + (randomDirection * popRadius);

        float elapsedTime = 0f;

        while (elapsedTime < popDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / popDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPos;

        yield return new WaitForSeconds(0.1f);

        canFly = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExperienceManager.Instance.AddXP(xpAmount);
            PoolManager.Instance.Return(prefab, gameObject);
        }
    }
}