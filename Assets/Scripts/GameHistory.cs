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

    private void OnEnable()
    {
        GameEvents.OnStockBought += (stock, quantity) =>
            AddEntry($"Kupiono {quantity} akcji {stock.stockData.stockName} po {stock.CurrentPrice:F2}");

        GameEvents.OnStockSold += (stock, quantity) =>
            AddEntry($"Sprzedano {quantity} akcji {stock.stockData.stockName} po {stock.CurrentPrice:F2}");

        GameEvents.OnMarketEventTriggered += @event =>
            AddEntry($"Zdarzenie: {@event.eventName} - {@event.description}");

        GameEvents.OnTurnEnded += turn =>
            AddEntry($"Zakończono turę {turn}");
    }

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
