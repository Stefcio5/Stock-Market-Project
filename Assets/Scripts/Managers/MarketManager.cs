using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private float _fluctuationFactor = 0.05f;
    public List<StockDataSO> stocks;
    public Dictionary<StockDataSO, Stock> Stocks { get; private set; } = new();


    public void InitializeStocks()
    {
        Stocks.Clear();

        foreach (var stockData in stocks)
        {
            Stocks[stockData] = new Stock(stockData);
        }
    }

    public void UpdateCurrentPrices()
    {
        foreach (var stock in Stocks.Values)
        {
            float fluctuation = 1f + Random.Range(-_fluctuationFactor, _fluctuationFactor);
            stock.UpdateCurrentPrice(fluctuation);
        }
    }

    public void CommitPriceChanges()
    {
        foreach (var stock in Stocks.Values)
        {
            stock.CommitPriceChange();
        }
    }

    public void SetPreviousPrices()
    {
        foreach (var stock in Stocks.Values)
        {
            stock.SetPreviousPrice();
        }
    }

    public Stock GetStock(StockDataSO stockData) => Stocks[stockData];
    public Stock GetRandomStock()
    {
        int randomIndex = Random.Range(0, Stocks.Count);
        return Stocks.ElementAt(randomIndex).Value;
    }
    public List<Stock> GetSectorStocks(SectorSO sector) => Stocks.Values
        .Where(stock => stock.stockData.sectors.Contains(sector))
        .ToList();
}
