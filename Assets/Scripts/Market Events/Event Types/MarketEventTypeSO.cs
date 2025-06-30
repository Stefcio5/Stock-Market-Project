using UnityEngine;

public abstract class MarketEventTypeSO : ScriptableObject
{
    public abstract void ApplyEventEffect(MarketManager marketManager, MarketEventSO marketEvent, float effect);

}
