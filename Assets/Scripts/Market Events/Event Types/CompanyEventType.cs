using UnityEngine;

[CreateAssetMenu(fileName = "CompanyEventType", menuName = "Scriptable Objects/MarketEventTypes/CompanyEventType")]
public class CompanyEventType : MarketEventTypeSO
{
    public StockDataSO stockData;

    public override void ApplyEventEffect(MarketManager marketManager, MarketEventSO marketEvent, float effect)
    {
        if (marketManager.Stocks.TryGetValue(stockData, out Stock stock))
        {
            stock.UpdatePrice(effect);
            GameEvents.RaiseOnMarketEventTriggered($"Wydarzenie firmy {stockData.stockName}", marketEvent);
        }
        else
        {
            Debug.LogWarning($"StockDataSO '{stockData.name}' not found in market manager.");
        }
    }
}
