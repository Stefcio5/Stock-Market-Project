using System;
using UnityEngine;

public class MarketUIController : MonoBehaviour
{
    [SerializeField] private GameObject stockUIPrefab;
    [SerializeField] private Transform stockUIContainer;

    public void GenerateStockUI(MarketManager marketManager, PlayerPortfolio playerPortfolio)
    {
        foreach (var stock in marketManager.Stocks.Values)
        {
            var stockUI = Instantiate(stockUIPrefab, stockUIContainer);
            stockUI.GetComponent<StockUI>().Init(stock, playerPortfolio);
            stockUI.name = $"{stock.stockData.stockName}";
        }
    }
}
