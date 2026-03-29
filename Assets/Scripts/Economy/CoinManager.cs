using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private float balance;
    public float Balance => balance;
    private float totalCoinsCollected;
    public float TotalCoinsCollected => totalCoinsCollected;

    public event Action OnBalanceChanged;

    public List<CoinPickup> activeCoins = new List<CoinPickup>();

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
        OnBalanceChanged?.Invoke();
    }

    public void SpendCoins(int amount)
    {
        balance -= amount;
        OnBalanceChanged?.Invoke();
    }

    public void RegisterCoin(CoinPickup coin)
    {
        if (!activeCoins.Contains(coin))
        {
            activeCoins.Add(coin);
        }
    }

    public void UnregisterCoin(CoinPickup coin)
    {
        activeCoins.Remove(coin);
    }
    public void TriggerMagnet(Transform playerTransform)
    {
        foreach (CoinPickup coin in activeCoins)
        {
            if (coin != null)
            {
                coin.StartMagnetMode(playerTransform);
            }
        }
    }
}
