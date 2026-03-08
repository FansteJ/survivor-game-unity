using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private float balance;
    public float Balance => balance;
    private float totalCoinsCollected;
    public float TotalCoinsCollected => totalCoinsCollected;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int value)
    {
        balance += value * PlayerStats.Instance.GoldMultiplier;
        totalCoinsCollected += value * PlayerStats.Instance.GoldMultiplier;
    }

    public void SpendCoins(int amount)
    {
        balance -= amount;
    }
}
