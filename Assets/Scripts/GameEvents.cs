using System;

public static class GameEvents
{
    public static event Action OnGameStarted;
    public static event Action OnGameEnded;

    public static event Action<Stock, int> OnStockBought;
    public static event Action<Stock, int> OnStockSold;
    public static event Action<float> OnCashChanged;
    public static event Action<int> OnTurnEnded;
    public static event Action<MarketEventSO> OnMarketEventTriggered;
    public static event Action OnNextTurnRequested;

    public static void RaiseOnGameStarted() => OnGameStarted?.Invoke();
    public static void RaiseOnGameEnded() => OnGameEnded?.Invoke();
    public static void RaiseOnStockBought(Stock stock, int quantity) => OnStockBought?.Invoke(stock, quantity);
    public static void RaiseOnStockSold(Stock stock, int quantity) => OnStockSold?.Invoke(stock, quantity);
    public static void RaiseOnCashChanged(float cash) => OnCashChanged?.Invoke(cash);
    public static void RaiseOnTurnEnded(int turn) => OnTurnEnded?.Invoke(turn);
    public static void RaiseOnMarketEventTriggered(MarketEventSO marketEvent) => OnMarketEventTriggered?.Invoke(marketEvent);
    public static void RaiseOnNextTurnRequested() => OnNextTurnRequested?.Invoke();

}
