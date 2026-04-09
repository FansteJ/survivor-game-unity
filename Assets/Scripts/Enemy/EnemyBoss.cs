using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private PlayerController player;
    private EnemyHealth myHealth;

    private float currentTime = 0f;
    private float lifeStealTaken;

    void Awake()
    {
        myHealth = GetComponent<EnemyHealth>();
    }

    void OnEnable()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerController>();
            lifeStealTaken = PlayerStats.Instance.LifeSteal / 2f;
            PlayerStats.Instance.AddLifeSteal(-lifeStealTaken);
        }

        if (BossUIManager.Instance != null)
        {
            BossUIManager.Instance.ShowBossUI();
        }

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowNotification("BOSS SPAWNED!\n<size=50%>50% lifesteal penalty applied, slow applies over time!</size>");
        }
    }

    void Update()
    {
        if (BossUIManager.Instance != null && myHealth != null)
        {
            BossUIManager.Instance.UpdateHP(myHealth.currentHealth, myHealth.maxHealth);
        }

        if (player == null) return;

        currentTime += Time.deltaTime;
        if (currentTime >= 1f)
        {
            player.Speed = player.Speed * 0.99f;
            currentTime = 0f;
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.Speed = 7f; // default value
        }

        if (BossUIManager.Instance != null)
        {
            BossUIManager.Instance.HideBossUI();
        }

        if (EnemySpawner.Instance != null && EnemySpawner.Instance.isWaitingForBossDeath)
        {
            EnemySpawner.Instance.AdvanceToNextLoop();
        }

        PlayerStats.Instance.AddLifeSteal(lifeStealTaken);

    }
}