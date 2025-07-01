using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvestmentAnalyzer : MonoBehaviour
{
    private List<TransactionEntry> _transactionHistory = new List<TransactionEntry>();
    private Dictionary<Stock, float> _stockInvestmentReturns = new Dictionary<Stock, float>();
    private MarketManager _marketManager;
    private PlayerPortfolio _playerPortfolio;

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
        _marketManager = marketManager;
        _playerPortfolio = playerPortfolio;
        _transactionHistory.Clear();
        _stockInvestmentReturns.Clear();
    }

    void OnEnable()
    {
        GameEvents.OnStockBought += AddBuyTransactionEntry;
        GameEvents.OnStockSold += AddSellTransactionEntry;
    }


    void OnDisable()
    {
        GameEvents.OnStockBought -= AddBuyTransactionEntry;
        GameEvents.OnStockSold -= AddSellTransactionEntry;
    }

    private void AddBuyTransactionEntry(Stock stock, int quantity)
    {
        var entry = new TransactionEntry(stock, quantity, stock.CurrentPrice, true);
        _transactionHistory.Add(entry);
    }
    private void AddSellTransactionEntry(Stock stock, int quantity)
    {
        var entry = new TransactionEntry(stock, quantity, stock.CurrentPrice, false);
        _transactionHistory.Add(entry);
    }

    public (Stock bestStock, float bestProfit, Stock worstStock, float worstProfit) CalculateBestAndWorstInvestmentReturn()
    {
        _stockInvestmentReturns.Clear();

        foreach (var stock in _marketManager.Stocks.Values)
        {
            float bought = _transactionHistory
                .FindAll(entry => entry.stock == stock && entry.isBought)
                .Sum(entry => entry.pricePerUnit * entry.quantity);

            float sold = _transactionHistory
                .FindAll(entry => entry.stock == stock && !entry.isBought)
                .Sum(entry => entry.pricePerUnit * entry.quantity);

            float currentValue = stock.CurrentPrice * _playerPortfolio.GetOwnedShares(stock.stockData);

            float profit = sold - bought + currentValue;
            _stockInvestmentReturns[stock] = profit;
        }

        if (_stockInvestmentReturns.Count == 0)
            return (null, 0f, null, 0f);

        var sorted = _stockInvestmentReturns.OrderBy(kv => kv.Value).ToList();

        var worst = sorted.First();
        var best = sorted.Last();

        Debug.Log($"Best Investment: {best.Key.stockData.stockName} with profit {best.Value:F2}");
        Debug.Log($"Worst Investment: {worst.Key.stockData.stockName} with profit {worst.Value:F2}");

        return (best.Key, best.Value, worst.Key, worst.Value);
    }
}
