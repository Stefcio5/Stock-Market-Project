using UnityEngine;

public class EventAlertUI : MonoBehaviour
{
    [SerializeField] private GameObject _alertPanel;
    [SerializeField] private TMPro.TextMeshProUGUI _alertText;

    private void OnEnable()
    {
        GameEvents.OnMarketEventTriggered += ShowAlert;
        GameEvents.OnTurnEnded += (t) => HideAlert();
    }
    private void OnDisable()
    {
        GameEvents.OnMarketEventTriggered -= ShowAlert;
        GameEvents.OnTurnEnded -= (t) => HideAlert();
    }

    private void ShowAlert(string eventType, MarketEventSO marketEvent)
    {
        if (_alertPanel.activeSelf)
        {
            HideAlert();
        }
        _alertPanel.SetActive(true);
        _alertText.text = $"{eventType}: {marketEvent.eventName} - {marketEvent.description}";
    }

    private void HideAlert()
    {
        _alertPanel.SetActive(false);
    }
}
