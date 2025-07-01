using System;
using UnityEngine;

public class MarketUIController : MonoBehaviour
{
    [SerializeField] private GameObject _stockUIPrefab;
    [SerializeField] private Transform _stockUIContentContainer;

    public void GenerateStockUI(MarketManager marketManager, PlayerPortfolio playerPortfolio)
    {
        foreach (var stock in marketManager.Stocks.Values)
        {
            var stockUI = Instantiate(_stockUIPrefab, _stockUIContentContainer);
            stockUI.GetComponent<StockUI>().Init(stock, playerPortfolio);
            stockUI.name = $"{stock.stockData.stockName}";
        }
    }
}
