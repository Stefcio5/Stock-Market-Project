using UnityEngine;

[CreateAssetMenu(fileName = "GlobalEventType", menuName = "Scriptable Objects/MarketEventTypes/GlobalEventType")]
public class GlobalEventType : MarketEventTypeSO
{
    public override void ApplyEventEffect(MarketManager marketManager, MarketEventSO marketEvent, float effect)
    {
        var allStocks = marketManager.Stocks.Values;

        foreach (var stock in allStocks)
        {
            stock.UpdatePrice(1f + effect);
        }
    }
}
