using UnityEngine;

[CreateAssetMenu(fileName = "GlobalEvent", menuName = "Scriptable Objects/MarketEventTypes/GlobalEvent")]
public class GlobalEvent : MarketEventTypeSO
{
    public override void ApplyEventEffect(MarketManager marketManager, MarketEventSO marketEvent, float effect)
    {
        throw new System.NotImplementedException();
    }
}
