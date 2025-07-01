using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHistory : MonoBehaviour
{
    public List<string> entries = new();

    [SerializeField] private Transform historyContent;
    [SerializeField] private GameObject historyItemPrefab;

    public static event Action OnEntryAdded;

    private Action _onGameStarted;
    private Action _onGameEnded;
    private Action<Stock, int> _onStockBought;
    private Action<Stock, int> _onStockSold;
    private Action<int> _onTurnEnded;
    private Action<string, MarketEventSO> _onMarketEventTriggered;

    private void Awake()
    {
        _onGameStarted = OnGameStarted;
        _onGameEnded = OnGameEnded;
        _onStockBought = OnStockBought;
        _onStockSold = OnStockSold;
        _onTurnEnded = OnTurnEnded;
        _onMarketEventTriggered = OnMarketEventTriggered;
    }

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
        GameEvents.OnGameStarted += _onGameStarted;
        GameEvents.OnGameEnded += _onGameEnded;
        GameEvents.OnStockBought += _onStockBought;
        GameEvents.OnStockSold += _onStockSold;
        GameEvents.OnTurnEnded += _onTurnEnded;
        GameEvents.OnMarketEventTriggered += _onMarketEventTriggered;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.OnGameStarted -= _onGameStarted;
        GameEvents.OnGameEnded -= _onGameEnded;
        GameEvents.OnStockBought -= _onStockBought;
        GameEvents.OnStockSold -= _onStockSold;
        GameEvents.OnTurnEnded -= _onTurnEnded;
        GameEvents.OnMarketEventTriggered -= _onMarketEventTriggered;
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
        entries.Add(text);
        var go = Instantiate(historyItemPrefab, historyContent);
        go.GetComponentInChildren<TMP_Text>().text = text;
        OnEntryAdded?.Invoke();
    }

    public void Clear()
    {
        entries.Clear();
    }

    public List<string> GetEntries()
    {
        return entries;
    }
}
