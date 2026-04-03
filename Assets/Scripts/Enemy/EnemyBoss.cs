using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    float currentTime = 0f;

    private PlayerController player;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (player == null) return;

        currentTime += Time.deltaTime;
        if(currentTime >= 1f)
        {
            player.speed = player.speed * 0.99f;
            currentTime = 0f;
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.speed = 7f; // default value
        }
    }
}
