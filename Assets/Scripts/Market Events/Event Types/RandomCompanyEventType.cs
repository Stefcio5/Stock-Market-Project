using UnityEngine;

[CreateAssetMenu(fileName = "RandomCompanyEventType", menuName = "Scriptable Objects/RandomCompanyEventType")]
public class RandomCompanyEventType : MarketEventTypeSO
{
    public override void ApplyEventEffect(MarketManager marketManager, MarketEventSO marketEvent, float effect)
    {
        var randomCompany = marketManager.GetRandomStock();
        if (randomCompany != null)
        {
            randomCompany.UpdatePrice(1f + effect);
            GameEvents.RaiseOnMarketEventTriggered($"Wydarzenie firmy {randomCompany.stockData.stockName}", marketEvent);
        }
        else
        {
            Debug.LogWarning("No stocks available in the market manager.");
        }
    }
}
