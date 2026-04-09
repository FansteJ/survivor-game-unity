using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    [Header("UI References")]
    public TMP_Text notificationText;
    public float displayDuration = 3f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (notificationText != null)
            notificationText.gameObject.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        if (notificationText == null) return;

        StopAllCoroutines();
        StartCoroutine(DisplayRoutine(message));
    }

    private IEnumerator DisplayRoutine(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        notificationText.gameObject.SetActive(false);
    }
}