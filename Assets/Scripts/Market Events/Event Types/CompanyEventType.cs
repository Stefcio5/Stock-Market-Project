using UnityEngine;

[CreateAssetMenu(fileName = "CompanyEventType", menuName = "Scriptable Objects/MarketEventTypes/CompanyEventType")]
public class CompanyEventType : MarketEventTypeSO
{
    public StockDataSO stockData;

    public override void ApplyEventEffect(MarketManager marketManager, MarketEventSO marketEvent, float effect)
    {
        if (marketManager.Stocks.TryGetValue(stockData, out Stock stock))
        {
            stock.UpdatePrice(1f + effect);
        }
        else
        {
            Debug.LogWarning($"StockDataSO '{stockData.name}' not found in market manager.");
        }
    }
}
