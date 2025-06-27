using UnityEngine;

public class PlayerPortfolio : MonoBehaviour
{
    public float playerCash = 10000f;
    private MarketManager marketManager;

    public void Init(MarketManager marketManager)
    {
        this.marketManager = marketManager;
    }

    public void BuyStock(StockDataSO stockData, int quantity)
    {
        var stock = marketManager.GetStock(stockData);
        float totalCost = stock.CurrentPrice * quantity;
        if (totalCost > playerCash)
        {
            Debug.LogWarning("Not enough cash to buy " + quantity + " shares of " + stockData.stockName);
            return;
        }
        stock.BuyShares(quantity, ref playerCash);
        Debug.Log("Bought " + quantity + "shares of " + stockData.stockName + ". Remaining cash: " + playerCash);
    }

    public void SellStock(StockDataSO stockData, int quantity)
    {
        var stock = marketManager.GetStock(stockData);
        if (stock.OwnedShares < quantity)
        {
            Debug.LogWarning("Not enough shares to sell " + quantity + " shares of " + stockData.stockName);
            return;
        }
        stock.SellShares(quantity, ref playerCash);
    }

    public bool CanBuyStock(StockDataSO stockData, int quantity)
    {
        var stock = marketManager.GetStock(stockData);
        float totalCost = stock.CurrentPrice * quantity;
        return totalCost <= playerCash;
    }

    public bool CanSellStock(StockDataSO stockData, int quantity)
    {
        var stock = marketManager.GetStock(stockData);
        return stock.OwnedShares >= quantity;
    }
}
