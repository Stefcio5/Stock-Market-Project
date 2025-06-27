using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MarketManager marketManager;
    [SerializeField] private PlayerPortfolio playerPortfolio;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private MarketUIController marketUIController;

    [SerializeField] private int maxTurns = 20;
    private int currentTurn = 1;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        marketManager.InitializeStocks();
        playerPortfolio.Init(marketManager);
        marketUIController.GenerateStockUI(marketManager, playerPortfolio);
    }

    public void NextTurn()
    {
        if (currentTurn > maxTurns)
        {
            EndGame();
        }

        currentTurn++;
        marketManager.UpdatePrices();
        eventManager.TryTriggerMarketEvent(marketManager);
    }

    private void EndGame()
    {
        throw new NotImplementedException();
    }
}
