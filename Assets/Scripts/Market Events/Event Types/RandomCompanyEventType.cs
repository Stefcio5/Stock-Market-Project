using UnityEngine;

[CreateAssetMenu(fileName = "RandomCompanyEventType", menuName = "Scriptable Objects/MarketEventTypes/RandomCompanyEventType")]
public class RandomCompanyEventType : MarketEventTypeSO
{
    public override void ApplyEventEffect(MarketManager marketManager, MarketEventSO marketEvent, float effect)
    {
        var randomCompany = marketManager.GetRandomStock();
        if (randomCompany != null)
        {
            randomCompany.UpdatePrice(effect);
            GameEvents.RaiseOnMarketEventTriggered($"Wydarzenie firmy {randomCompany.stockData.stockName}", marketEvent);
        }
        else
        {
            Debug.LogWarning("No stocks available in the market manager.");
        }
    }
}
