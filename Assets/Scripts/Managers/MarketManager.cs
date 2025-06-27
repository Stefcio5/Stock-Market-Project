using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public List<StockDataSO> stocks;
    public Dictionary<StockDataSO, Stock> Stocks { get; private set; } = new();
    public float fluctuationFactor = 0.05f;


    public void InitializeStocks()
    {
        Stocks.Clear();

        foreach (var stockData in stocks)
        {
            Stocks[stockData] = new Stock(stockData);
        }
    }

    public void UpdatePrices()
    {
        foreach (var stock in Stocks.Values)
        {
            float fluctuation = 1f + Random.Range(-fluctuationFactor, fluctuationFactor);
            stock.UpdatePrice(fluctuation);
        }
    }

    public Stock GetStock(StockDataSO stockData) => Stocks[stockData];
    public List<Stock> GetSectorStocks(SectorSO sector) => Stocks.Values
        .Where(stock => stock.stockData.sectors.Contains(sector))
        .ToList();
}
