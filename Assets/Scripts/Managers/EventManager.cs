using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public List<MarketEventSO> marketEvents;
    [SerializeField] private int _eventFrequency = 3;

    private int _turnsSinceLastEvent = 0;

    public bool TryTriggerMarketEvent(MarketManager marketManager)
    {
        _turnsSinceLastEvent++;

        if (_turnsSinceLastEvent >= _eventFrequency)
        {
            _turnsSinceLastEvent = 0;

            if (marketEvents.Count > 0)
            {
                int randomIndex = Random.Range(0, marketEvents.Count);
                MarketEventSO selectedEvent = marketEvents[randomIndex];
                selectedEvent.Apply(marketManager);
                Debug.Log($"Market Event Triggered: {selectedEvent.eventName}");
                return true;
            }
        }
        return false;
    }
}
