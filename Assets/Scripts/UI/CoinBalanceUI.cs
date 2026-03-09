using UnityEngine;
using TMPro;

public class CoinBalanceUI : MonoBehaviour
{
    public TMP_Text goldText;

    private void Start()
    {
        CoinManager.Instance.OnBalanceChanged += UpdateBalance;
        UpdateBalance();
    }

    void UpdateBalance()
    {
        goldText.text = CoinManager.Instance.Balance.ToString("F0");
    }

    private void OnDestroy()
    {
        CoinManager.Instance.OnBalanceChanged -= UpdateBalance;
    }
}
