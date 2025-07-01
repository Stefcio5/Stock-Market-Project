using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MarketManager _marketManager;
    [SerializeField] private PlayerPortfolio _playerPortfolio;
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private InvestmentAnalyzer _investmentAnalyzer;
    [SerializeField] private MarketUIController _marketUIController;

    [SerializeField] private int _maxTurns = 20;
    private int _currentTurn = 1;

    private bool _isGameOver = false;

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
        _marketManager.InitializeStocks();
        _playerPortfolio.Init(_marketManager);
        _investmentAnalyzer.Init(_marketManager, _playerPortfolio);
        _marketUIController.GenerateStockUI(_marketManager, _playerPortfolio);
        GameEvents.RaiseOnCashChanged(_playerPortfolio.PlayerCash);
    }

    private void NextTurn()
    {
        if (_isGameOver) return;

        if (_currentTurn >= _maxTurns)
        {
            _isGameOver = true;
            EndGame();
            return;
        }
        GameEvents.RaiseOnTurnEnded(_currentTurn);
        _marketManager.SetPreviousPrices();
        _eventManager.TryTriggerMarketEvent(_marketManager);
        _marketManager.UpdateCurrentPrices();
        _marketManager.CommitPriceChanges();
        _currentTurn++;
    }

    private void EndGame()
    {
        _isGameOver = true;
        GameEvents.RaiseOnGameEnded();
        Debug.Log("Game Over! Total Turns: " + _currentTurn);
    }
}
