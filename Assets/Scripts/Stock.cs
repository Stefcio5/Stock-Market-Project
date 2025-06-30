using System;

public class Stock
{
    public StockDataSO stockData;
    public float CurrentPrice { get; private set; }
    public float PreviousPrice { get; private set; }
    public int OwnedShares { get; private set; }

    public event Action OnPriceChanged;

    public Stock(StockDataSO data)
    {
        stockData = data;
        CurrentPrice = stockData.basePrice;
        OwnedShares = 0;
    }

    public void UpdatePrice(float modifier)
    {
        PreviousPrice = CurrentPrice;
        CurrentPrice = Math.Max(1f, CurrentPrice * modifier);
        OnPriceChanged?.Invoke();
    }

    public float GetTrend()
    {
        if (PreviousPrice == 0)
            return 0;
        return (CurrentPrice - PreviousPrice) / PreviousPrice;
    }

    public void BuyShares(int amount, ref float playerCash)
    {
        float totalCost = CurrentPrice * amount;
        if (playerCash >= totalCost)
        {
            OwnedShares += amount;
            playerCash -= totalCost;
        }
    }

    public void SellShares(int amount, ref float playerCash)
    {
        if (OwnedShares >= amount)
        {
            OwnedShares -= amount;
            playerCash += CurrentPrice * amount;
        }
    }
}
