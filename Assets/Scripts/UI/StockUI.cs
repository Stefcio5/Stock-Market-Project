using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _stockNameText;
    [SerializeField] private TextMeshProUGUI _trendText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _ownedText;
    [SerializeField] private TextMeshProUGUI _averageBuyPriceText;

    [SerializeField] private TMP_InputField _quantityInput;
    [SerializeField] private Button _minusButton;
    [SerializeField] private Button _plusButton;

    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;

    private Stock _stock;
    private PlayerPortfolio _portfolio;

    private int _quantity = 1;


    public void Init(Stock stock, PlayerPortfolio playerPortfolio)
    {
        _stock = stock;
        _portfolio = playerPortfolio;
        _stockNameText.text = stock.stockData.stockName;
        _quantityInput.text = _quantity.ToString();
        AddButtonListeners();
        UpdateUI();
        _stock.OnPriceChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (_stock != null)
        {
            _stock.OnPriceChanged -= UpdateUI;
        }
        RemoveButtonListeners();
    }

    private void AddButtonListeners()
    {
        _buyButton.onClick.AddListener(OnBuyButtonClicked);
        _sellButton.onClick.AddListener(OnSellButtonClicked);
        _plusButton.onClick.AddListener(OnPlusButtonClicked);
        _minusButton.onClick.AddListener(OnMinusButtonClicked);
        _quantityInput.onValueChanged.AddListener(OnQuantityInputChanged);
    }

    private void RemoveButtonListeners()
    {
        _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        _sellButton.onClick.RemoveListener(OnSellButtonClicked);
        _plusButton.onClick.RemoveListener(OnPlusButtonClicked);
        _minusButton.onClick.RemoveListener(OnMinusButtonClicked);
        _quantityInput.onValueChanged.RemoveListener(OnQuantityInputChanged);
    }

    private void OnBuyButtonClicked()
    {
        _portfolio.BuyStock(_stock.stockData, _quantity);
        UpdateUI();
    }

    private void OnSellButtonClicked()
    {
        _portfolio.SellStock(_stock.stockData, _quantity);
        UpdateUI();
    }

    private void OnPlusButtonClicked()
    {
        _quantity++;
        OnQuantityInputChanged(_quantity.ToString());
        UpdateUI();
    }

    private void OnMinusButtonClicked()
    {
        if (_quantity <= 1) return;
        _quantity--;
        OnQuantityInputChanged(_quantity.ToString());
        UpdateUI();
    }

    private void OnQuantityInputChanged(string arg0)
    {
        if (int.TryParse(arg0, out int newQuantity) && newQuantity > 0)
        {
            _quantity = newQuantity;
            _quantityInput.text = _quantity.ToString();
            UpdateUI();
        }
        else
        {
            _quantity = 1;
            _quantityInput.text = "1";
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        _priceText.text = $"Cena: ${_stock.CurrentPrice:F2}";
        _ownedText.text = $"Posiadane akcje: {_portfolio.GetOwnedShares(_stock.stockData)}";
        _averageBuyPriceText.text = $"Średnia cena zakupu: ${_portfolio.GetAverageBuyPrice(_stock.stockData):F2}";
        SetTrendText(_stock.GetTrend());
        _buyButton.interactable = _portfolio.CanBuyStock(_stock.stockData, _quantity);
        _sellButton.interactable = _portfolio.CanSellStock(_stock.stockData, _quantity);
    }

    private void SetTrendText(float trendValue)
    {
        if (trendValue > 0)
        {
            _trendText.color = Color.green;
            _trendText.text = $"+{trendValue * 100:F2}%";
        }
        else if (trendValue < 0)
        {
            _trendText.color = Color.red;
            _trendText.text = $"{trendValue * 100:F2}%";
        }
        else
        {
            _trendText.color = Color.white;
        }
    }
}
