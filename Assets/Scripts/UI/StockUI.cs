using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI stockNameText;
    // public Image trendIcon;
    // public Sprite upArrow;
    // public Sprite downArrow;
    public TextMeshProUGUI trendText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI ownedText;

    public TMP_InputField quantityInput;
    public Button minusButton;
    public Button plusButton;

    public Button buyButton;
    public Button sellButton;

    private Stock stockInstance;
    private PlayerPortfolio portfolio;

    private int quantity = 1;

    public void Init(Stock stock, PlayerPortfolio playerPortfolio)
    {
        stockInstance = stock;
        portfolio = playerPortfolio;
        stockNameText.text = stock.stockData.stockName;
        quantityInput.text = quantity.ToString();
        SetupButtons();
        UpdateUI();
    }

    private void SetupButtons()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        sellButton.onClick.AddListener(OnSellButtonClicked);
        plusButton.onClick.AddListener(OnPlusButtonClicked);
        minusButton.onClick.AddListener(OnMinusButtonClicked);
        quantityInput.onValueChanged.AddListener(OnQuantityInputChanged);
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
        ownedText.text = $"Posiadane akcje: {stockInstance.OwnedShares}";
        buyButton.interactable = portfolio.CanBuyStock(stockInstance.stockData, quantity);
        sellButton.interactable = portfolio.CanSellStock(stockInstance.stockData, quantity);
    }
}
