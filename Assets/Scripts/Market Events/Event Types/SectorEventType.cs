using UnityEngine;

[CreateAssetMenu(fileName = "SectorEventType", menuName = "Scriptable Objects/MarketEventTypes/SectorEventType")]
public class SectorEventType : MarketEventTypeSO
{
    public SectorSO sector;
    public override void ApplyEventEffect(MarketManager marketManager, MarketEventSO marketEvent, float effect)
    {
        var sectorStocks = marketManager.GetSectorStocks(sector);
        foreach (var stock in sectorStocks)
        {
            stock.UpdateCurrentPrice(effect);
        }
        GameEvents.RaiseOnMarketEventTriggered($"Wydarzenie sektora {sector.name}", marketEvent);
    }
}
