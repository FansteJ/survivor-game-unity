using UnityEngine;
using TMPro;

public class CoinBalanceUI : MonoBehaviour
{
    public TMP_Text goldText;

    // Update is called once per frame
    void Update()
    {
        goldText.text = Mathf.FloorToInt(CoinManager.Instance.Balance).ToString();
    }
}
