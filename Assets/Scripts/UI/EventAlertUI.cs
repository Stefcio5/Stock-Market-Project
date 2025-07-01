using UnityEngine;
using DG.Tweening;
using System;

public class EventAlertUI : MonoBehaviour
{
    [SerializeField] private GameObject _alertPanel;
    [SerializeField] private TMPro.TextMeshProUGUI _alertText;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;

    [SerializeField] private float _alertDuration = 0.5f;
    private Tween _currentTween;

    private void OnEnable()
    {
        GameEvents.OnMarketEventTriggered += ShowAlertNotification;
        GameEvents.OnTurnEnded += OnTurnEnded;
    }
    private void OnDisable()
    {
        GameEvents.OnMarketEventTriggered -= ShowAlertNotification;
        GameEvents.OnTurnEnded -= OnTurnEnded;
    }
    private void ShowAlertNotification(string eventType, MarketEventSO marketEvent)
    {
        if (_alertPanel.activeSelf || _currentTween != null && _currentTween.IsActive())
        {
            HideAlertNotification(() => ShowAlert(eventType, marketEvent));
        }
        else
        {
            ShowAlert(eventType, marketEvent);
        }
    }

    private void ShowAlert(string eventType, MarketEventSO marketEvent)
    {
        _alertPanel.transform.position = _startPosition.position;
        _alertPanel.SetActive(true);
        _alertText.text = $"{eventType}: {marketEvent.eventName} - {marketEvent.description}";
        _currentTween = _alertPanel.transform.DOLocalMove(_endPosition.localPosition, _alertDuration)
            .SetEase(Ease.OutBack);
    }

    private void OnTurnEnded(int turn)
    {
        HideAlertNotification();
    }

    private void HideAlertNotification(Action onComplete = null)
    {
        if (_alertPanel.activeSelf)
        {
            _currentTween = _alertPanel.transform.DOLocalMove(_startPosition.localPosition, _alertDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _alertPanel.SetActive(false);
                    onComplete?.Invoke();
                });
        }
    }
}

