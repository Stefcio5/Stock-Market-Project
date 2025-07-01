using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHistory : MonoBehaviour
{

    [SerializeField] private Transform _historyContentTransform;
    [SerializeField] private GameObject _historyContentPrefab;
    private List<string> _entries = new();

    public static event Action OnEntryAdded;

    private void OnEnable()
    {
        SubscribeToEvents();
        Clear();
    }

    private void OnDisable()
    {
        UnSubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.OnGameStarted += OnGameStarted;
        GameEvents.OnGameEnded += OnGameEnded;
        GameEvents.OnStockBought += OnStockBought;
        GameEvents.OnStockSold += OnStockSold;
        GameEvents.OnTurnEnded += OnTurnEnded;
        GameEvents.OnMarketEventTriggered += OnMarketEventTriggered;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.OnGameStarted -= OnGameStarted;
        GameEvents.OnGameEnded -= OnGameEnded;
        GameEvents.OnStockBought -= OnStockBought;
        GameEvents.OnStockSold -= OnStockSold;
        GameEvents.OnTurnEnded -= OnTurnEnded;
        GameEvents.OnMarketEventTriggered -= OnMarketEventTriggered;
    }

    private void OnGameStarted() => AddEntry("Rozpoczęto grę");
    private void OnGameEnded() => AddEntry("Zakończono grę");
    private void OnStockBought(Stock stock, int quantity) =>
        AddEntry($"Kupiono {quantity} akcji {stock.stockData.stockName} po {stock.CurrentPrice:F2}");
    private void OnStockSold(Stock stock, int quantity) =>
        AddEntry($"Sprzedano {quantity} akcji {stock.stockData.stockName} po {stock.CurrentPrice:F2}");
    private void OnTurnEnded(int turn) => AddEntry($"Zakończono turę {turn}");
    private void OnMarketEventTriggered(string eventType, MarketEventSO marketEvent) =>
        AddEntry($"{eventType}: {marketEvent.eventName} - {marketEvent.description}");

    public void AddEntry(string text)
    {
        _entries.Add(text);
        var go = Instantiate(_historyContentPrefab, _historyContentTransform);
        go.GetComponentInChildren<TMP_Text>().text = text;
        OnEntryAdded?.Invoke();
    }

    public void Clear()
    {
        _entries.Clear();
    }

    public List<string> GetEntries()
    {
        return _entries;
    }
}
