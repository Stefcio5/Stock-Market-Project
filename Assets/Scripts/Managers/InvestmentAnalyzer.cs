using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InvestmentAnalyzer : MonoBehaviour
{
    [SerializeField] private List<TransactionEntry> transactionHistory = new List<TransactionEntry>();
    [SerializeField] private Dictionary<Stock, float> stockInvestmentReturns = new Dictionary<Stock, float>();
    private MarketManager marketManager;
    private PlayerPortfolio playerPortfolio;

    public static InvestmentAnalyzer Instance;

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
    }

    public void Init(MarketManager marketManager, PlayerPortfolio playerPortfolio)
    {
        this.marketManager = marketManager;
        this.playerPortfolio = playerPortfolio;
        transactionHistory.Clear();
        stockInvestmentReturns.Clear();
    }

    void OnEnable()
    {
        GameEvents.OnStockBought += AddBuyTransactionEntry;
        GameEvents.OnStockSold += AddSellTransactionEntry;
        GameEvents.OnGameEnded += () => CalculateBestAndWorstInvestmentReturn();
    }


    void OnDisable()
    {
        GameEvents.OnStockBought -= AddBuyTransactionEntry;
        GameEvents.OnStockSold -= AddSellTransactionEntry;
        GameEvents.OnGameEnded -= () => CalculateBestAndWorstInvestmentReturn();
    }

    private void AddBuyTransactionEntry(Stock stock, int quantity)
    {
        var entry = new TransactionEntry(stock, quantity, stock.CurrentPrice, true);
        transactionHistory.Add(entry);
    }
    private void AddSellTransactionEntry(Stock stock, int quantity)
    {
        var entry = new TransactionEntry(stock, quantity, stock.CurrentPrice, false);
        transactionHistory.Add(entry);
    }

    public (Stock bestStock, float bestProfit, Stock worstStock, float worstProfit) CalculateBestAndWorstInvestmentReturn()
    {
        stockInvestmentReturns.Clear();

        foreach (var stock in marketManager.Stocks.Values)
        {
            float bought = transactionHistory
                .FindAll(entry => entry.stock == stock && entry.isBought)
                .Sum(entry => entry.pricePerUnit * entry.quantity);

            float sold = transactionHistory
                .FindAll(entry => entry.stock == stock && !entry.isBought)
                .Sum(entry => entry.pricePerUnit * entry.quantity);

            float currentValue = stock.CurrentPrice * playerPortfolio.GetOwnedShares(stock.stockData);

            float profit = sold - bought + currentValue;
            stockInvestmentReturns[stock] = profit;
        }

        if (stockInvestmentReturns.Count == 0)
            return (null, 0f, null, 0f);

        var sorted = stockInvestmentReturns.OrderBy(kv => kv.Value).ToList();

        var worst = sorted.First();
        var best = sorted.Last();

        Debug.Log($"Best Investment: {best.Key.stockData.stockName} with profit {best.Value:F2}");
        Debug.Log($"Worst Investment: {worst.Key.stockData.stockName} with profit {worst.Value:F2}");

        return (best.Key, best.Value, worst.Key, worst.Value);
    }
}
