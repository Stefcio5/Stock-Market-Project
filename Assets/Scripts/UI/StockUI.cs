using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI stockNameText;
    [SerializeField] private TextMeshProUGUI trendText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI ownedText;
    [SerializeField] private TextMeshProUGUI averageBuyPriceText;

    [SerializeField] private TMP_InputField quantityInput;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;

    [SerializeField] private Button buyButton;
    [SerializeField] private Button sellButton;

    private Stock stockInstance;
    private PlayerPortfolio portfolio;

    private int quantity = 1;


    public void Init(Stock stock, PlayerPortfolio playerPortfolio)
    {
        stockInstance = stock;
        portfolio = playerPortfolio;
        stockNameText.text = stock.stockData.stockName;
        quantityInput.text = quantity.ToString();
        AddButtonListeners();
        UpdateUI();
        stockInstance.OnPriceChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (stockInstance != null)
        {
            stockInstance.OnPriceChanged -= UpdateUI;
        }
        RemoveButtonListeners();
    }

    private void AddButtonListeners()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        sellButton.onClick.AddListener(OnSellButtonClicked);
        plusButton.onClick.AddListener(OnPlusButtonClicked);
        minusButton.onClick.AddListener(OnMinusButtonClicked);
        quantityInput.onValueChanged.AddListener(OnQuantityInputChanged);
    }

    private void RemoveButtonListeners()
    {
        buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        sellButton.onClick.RemoveListener(OnSellButtonClicked);
        plusButton.onClick.RemoveListener(OnPlusButtonClicked);
        minusButton.onClick.RemoveListener(OnMinusButtonClicked);
        quantityInput.onValueChanged.RemoveListener(OnQuantityInputChanged);
    }

    private void OnBuyButtonClicked()
    {
        portfolio.BuyStock(stockInstance.stockData, quantity);
        UpdateUI();
    }

    private void OnSellButtonClicked()
    {
        portfolio.SellStock(stockInstance.stockData, quantity);
        UpdateUI();
    }

    private void OnPlusButtonClicked()
    {
        quantity++;
        OnQuantityInputChanged(quantity.ToString());
        UpdateUI();
    }

    private void OnMinusButtonClicked()
    {
        if (quantity <= 1) return;
        quantity--;
        OnQuantityInputChanged(quantity.ToString());
        UpdateUI();
    }

    private void OnQuantityInputChanged(string arg0)
    {
        if (int.TryParse(arg0, out int newQuantity) && newQuantity > 0)
        {
            quantity = newQuantity;
            quantityInput.text = quantity.ToString();
            UpdateUI();
        }
        else
        {
            quantity = 1;
            quantityInput.text = "1";
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        priceText.text = $"Cena: ${stockInstance.CurrentPrice:F2}";
        ownedText.text = $"Posiadane akcje: {portfolio.GetOwnedShares(stockInstance.stockData)}";
        averageBuyPriceText.text = $"Średnia cena zakupu: ${portfolio.GetAverageBuyPrice(stockInstance.stockData):F2}";
        SetTrendText(stockInstance.GetTrend());
        buyButton.interactable = portfolio.CanBuyStock(stockInstance.stockData, quantity);
        sellButton.interactable = portfolio.CanSellStock(stockInstance.stockData, quantity);
    }

    private void SetTrendText(float trendValue)
    {
        if (trendValue > 0)
        {
            trendText.color = Color.green;
            trendText.text = $"+{trendValue * 100:F2}%";
        }
        else if (trendValue < 0)
        {
            trendText.color = Color.red;
            trendText.text = $"{trendValue * 100:F2}%";
        }
        else
        {
            trendText.color = Color.white;
        }
    }
}
