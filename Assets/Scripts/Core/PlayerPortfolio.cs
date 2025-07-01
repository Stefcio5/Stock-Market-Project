using System.Collections.Generic;
using UnityEngine;

public class PlayerPortfolio : MonoBehaviour
{
    [SerializeField] private float _initialCash = 10000f;
    private float _playerCash;
    public float PlayerCash
    {
        get => _playerCash;
        private set => _playerCash = value;
    }
    private Dictionary<StockDataSO, int> _ownedStocks = new();
    private Dictionary<StockDataSO, float> _totalSpentPerStock = new();
    private Dictionary<StockDataSO, int> _totalBoughtPerStock = new();
    private MarketManager _marketManager;

    public void Init(MarketManager marketManager)
    {
        _marketManager = marketManager;
        _playerCash = _initialCash;
        InitOwnedStocks();
    }

    private void InitOwnedStocks()
    {
        _ownedStocks.Clear();
        _totalSpentPerStock.Clear();
        _totalBoughtPerStock.Clear();
        foreach (var stock in _marketManager.stocks)
        {
            _ownedStocks[stock] = 0;
            _totalSpentPerStock[stock] = 0f;
            _totalBoughtPerStock[stock] = 0;
        }
    }

    public void BuyStock(StockDataSO stockData, int quantity)
    {
        var stock = _marketManager.GetStock(stockData);
        float totalCost = stock.CurrentPrice * quantity;
        if (totalCost > PlayerCash)
        {
            Debug.LogWarning("Not enough cash to buy " + quantity + " shares of " + stockData.stockName);
            return;
        }
        stock.BuyShares(quantity, ref _playerCash);
        _ownedStocks[stockData] += quantity;
        _totalSpentPerStock[stockData] += totalCost;
        _totalBoughtPerStock[stockData] += quantity;

        GameEvents.RaiseOnCashChanged(PlayerCash);
        GameEvents.RaiseOnStockBought(stock, quantity);
    }

    public void SellStock(StockDataSO stockData, int quantity)
    {
        var stock = _marketManager.GetStock(stockData);
        if (_ownedStocks[stockData] < quantity)
        {
            Debug.LogWarning("Not enough shares to sell " + quantity + " shares of " + stockData.stockName);
            return;
        }
        stock.SellShares(quantity, ref _playerCash);
        _ownedStocks[stockData] -= quantity;
        float avgBuyPrice = GetAverageBuyPrice(stockData);
        _totalSpentPerStock[stockData] -= avgBuyPrice * quantity;
        _totalBoughtPerStock[stockData] -= quantity;

        if (_ownedStocks[stockData] == 0)
        {
            _totalSpentPerStock[stockData] = 0f;
            _totalBoughtPerStock[stockData] = 0;
        }

        GameEvents.RaiseOnCashChanged(PlayerCash);
        GameEvents.RaiseOnStockSold(stock, quantity);
    }

    public bool CanBuyStock(StockDataSO stockData, int quantity)
    {
        var stock = _marketManager.GetStock(stockData);
        float totalCost = stock.CurrentPrice * quantity;
        return totalCost <= PlayerCash;
    }

    public bool CanSellStock(StockDataSO stockData, int quantity)
    {
        return _ownedStocks[stockData] >= quantity;
    }

    public float GetTotalValue()
    {
        return PlayerCash + GetTotalStockValue();
    }

    public float GetTotalStockValue()
    {
        float totalStockValue = 0f;
        foreach (var stock in _marketManager.Stocks.Values)
        {
            totalStockValue += stock.CurrentPrice * _ownedStocks.GetValueOrDefault(stock.stockData, 0);
        }
        return totalStockValue;
    }


    public float GetProfitPercentage()
    {
        float totalValue = GetTotalValue();
        return (totalValue - _initialCash) / _initialCash * 100f;
    }

    public int GetOwnedShares(StockDataSO stockData)
    {
        return _ownedStocks.TryGetValue(stockData, out int shares) ? shares : 0;
    }

    public float GetAverageBuyPrice(StockDataSO stockData)
    {
        int owned = _ownedStocks.TryGetValue(stockData, out int shares) ? shares : 0;
        if (owned == 0) return 0f;
        float spent = _totalSpentPerStock.TryGetValue(stockData, out float total) ? total : 0f;
        int bought = _totalBoughtPerStock.TryGetValue(stockData, out int totalBought) ? totalBought : 0;
        return bought > 0 ? spent / bought : 0f;
    }
}
