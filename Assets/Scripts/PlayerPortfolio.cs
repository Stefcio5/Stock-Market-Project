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
    private MarketManager marketManager;

    public void Init(MarketManager marketManager)
    {
        this.marketManager = marketManager;
        _playerCash = _initialCash;
    }

    public void BuyStock(StockDataSO stockData, int quantity)
    {
        var stock = marketManager.GetStock(stockData);
        float totalCost = stock.CurrentPrice * quantity;
        if (totalCost > PlayerCash)
        {
            Debug.LogWarning("Not enough cash to buy " + quantity + " shares of " + stockData.stockName);
            return;
        }
        stock.BuyShares(quantity, ref _playerCash);

        GameEvents.RaiseOnCashChanged(PlayerCash);
        GameEvents.RaiseOnStockBought(stock, quantity);
        Debug.Log("Bought " + quantity + "shares of " + stockData.stockName + ". Remaining cash: " + PlayerCash);
    }

    public void SellStock(StockDataSO stockData, int quantity)
    {
        var stock = marketManager.GetStock(stockData);
        if (stock.OwnedShares < quantity)
        {
            Debug.LogWarning("Not enough shares to sell " + quantity + " shares of " + stockData.stockName);
            return;
        }
        stock.SellShares(quantity, ref _playerCash);

        GameEvents.RaiseOnCashChanged(PlayerCash);
        GameEvents.RaiseOnStockSold(stock, quantity);
    }

    public bool CanBuyStock(StockDataSO stockData, int quantity)
    {
        var stock = marketManager.GetStock(stockData);
        float totalCost = stock.CurrentPrice * quantity;
        return totalCost <= PlayerCash;
    }

    public bool CanSellStock(StockDataSO stockData, int quantity)
    {
        var stock = marketManager.GetStock(stockData);
        return stock.OwnedShares >= quantity;
    }

    public float GetTotalStockValue()
    {
        float totalStockValue = 0f;
        foreach (var stock in marketManager.Stocks.Values)
        {
            totalStockValue += stock.CurrentPrice * stock.OwnedShares;
        }
        return totalStockValue;
    }

    public float GetTotalValue()
    {
        return PlayerCash + GetTotalStockValue();
    }

    public float GetProfitPercentage()
    {
        float totalValue = GetTotalValue();
        return (totalValue - _initialCash) / _initialCash * 100f;
    }
}
