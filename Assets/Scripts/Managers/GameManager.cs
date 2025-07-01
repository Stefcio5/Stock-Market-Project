using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MarketManager marketManager;
    [SerializeField] private PlayerPortfolio playerPortfolio;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private InvestmentAnalyzer investmentAnalyzer;
    [SerializeField] private MarketUIController marketUIController;

    [SerializeField] private int maxTurns = 20;
    private int currentTurn = 1;

    private bool isGameOver = false;

    private void OnEnable()
    {
        GameEvents.OnNextTurnRequested += NextTurn;
    }

    private void OnDisable()
    {
        GameEvents.OnNextTurnRequested -= NextTurn;
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        marketManager.InitializeStocks();
        playerPortfolio.Init(marketManager);
        investmentAnalyzer.Init(marketManager, playerPortfolio);
        marketUIController.GenerateStockUI(marketManager, playerPortfolio);
        GameEvents.RaiseOnCashChanged(playerPortfolio.PlayerCash);
    }

    private void NextTurn()
    {
        if (isGameOver)
        {
            return;
        }
        if (currentTurn >= maxTurns)
        {
            isGameOver = true;
            EndGame();
            return;
        }
        GameEvents.RaiseOnTurnEnded(currentTurn);
        marketManager.SetPreviousPrices();
        eventManager.TryTriggerMarketEvent(marketManager);
        marketManager.UpdateCurrentPrices();
        marketManager.CommitPriceChanges();
        currentTurn++;
    }

    private void EndGame()
    {
        isGameOver = true;
        GameEvents.RaiseOnGameEnded();
        Debug.Log("Game Over! Total Turns: " + currentTurn);
    }
}
