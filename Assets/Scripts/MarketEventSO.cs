using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MarketEventSO", menuName = "Scriptable Objects/MarketEvent/MarketEventSO")]
public class MarketEventSO : ScriptableObject
{
    public string eventName;
    public string description;
    public MarketEventTypeSO eventType;
    public float minImpactFactor;
    public float maxImpactFactor;

    public void Apply(MarketManager marketManager)
    {
        float effectValue = 1f + Random.Range(minImpactFactor, maxImpactFactor);
        if (eventType != null)
        {
            eventType.ApplyEventEffect(marketManager, this, effectValue);
        }
        else
        {
            Debug.LogWarning($"MarketEventSO '{eventName}' has no event type assigned.");
        }
    }
}
