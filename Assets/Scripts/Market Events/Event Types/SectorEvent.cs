using UnityEngine;

[CreateAssetMenu(fileName = "SectorEvent", menuName = "Scriptable Objects/MarketEventTypes/SectorEvent")]
public class SectorEvent : MarketEventTypeSO
{
    public override void ApplyEventEffect(MarketManager marketManager, MarketEventSO marketEvent, float effect)
    {
        throw new System.NotImplementedException();
    }
}
