using System;
using UnityEngine;

public class MarketUIController : MonoBehaviour
{
    [SerializeField] private GameObject stockUIPrefab;
    [SerializeField] private Transform stockUIContainer;

    [SerializeField] private MarketManager marketManager;
    [SerializeField] private PlayerPortfolio playerPortfolio;

    private void Start()
    {
        GenerateStockUI();
    }

    private void GenerateStockUI()
    {
        foreach (var stock in marketManager.Stocks.Values)
        {
            var stockUI = Instantiate(stockUIPrefab, stockUIContainer);
            stockUI.GetComponent<StockUI>().Init(stock, playerPortfolio);
            stockUI.name = $"{stock.stockData.stockName}";
        }
    }
}
