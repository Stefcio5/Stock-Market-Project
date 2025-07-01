using System;
using UnityEngine;

public class Stock
{
    public StockDataSO stockData;
    public float CurrentPrice { get; private set; }
    public float PreviousPrice { get; private set; }

    public int Demand { get; private set; }
    public int Supply { get; private set; }

    private float _demandFactorModifier = 0.1f;

    public event Action OnPriceChanged;

    public Stock(StockDataSO data)
    {
        stockData = data;
        CurrentPrice = stockData.basePrice;
        PreviousPrice = CurrentPrice;
        Demand = stockData.startingShares;
        Supply = stockData.startingShares;
    }

    public void UpdateCurrentPrice(float modifier)
    {
        float demandSupplyRatio = CalculateDemandSupplyRatio();
        float demandFactor = 1f + ((demandSupplyRatio - 1f) * _demandFactorModifier);

        CurrentPrice = Math.Max(1f, CurrentPrice * demandFactor);
        Debug.Log($"Demand Factor: {demandFactor}");

        CurrentPrice = Math.Max(1f, CurrentPrice * modifier);

        ResetDemandAndSupply();
    }

    public void CommitPriceChange()
    {
        OnPriceChanged?.Invoke();
    }

    public void SetPreviousPrice()
    {
        PreviousPrice = CurrentPrice;
    }

    public float GetTrend()
    {
        return (CurrentPrice - PreviousPrice) / PreviousPrice;
    }

    public void BuyShares(int amount, ref float playerCash)
    {
        float totalCost = CurrentPrice * amount;
        if (playerCash >= totalCost)
        {
            playerCash -= totalCost;
        }
        Demand += amount;
        Supply = Mathf.Max(0, Supply - amount);
    }

    public void SellShares(int amount, ref float playerCash)
    {
        playerCash += CurrentPrice * amount;
        Demand = Mathf.Max(0, Demand - amount);
        Supply += amount;
    }

    public void ResetDemandAndSupply()
    {
        Demand = stockData.startingShares;
        Supply = stockData.startingShares;
    }

    private float CalculateDemandSupplyRatio()
    {
        return Supply > 0 ? (float)Demand / Supply : 1f;
    }
}
