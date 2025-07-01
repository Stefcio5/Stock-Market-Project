using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryUI : MonoBehaviour
{
    [SerializeField] private GameHistory _gameHistory;
    [SerializeField] private PlayerPortfolio _playerPortfolio;
    [SerializeField] private GameObject _summaryPanel;
    [SerializeField] private TMP_Text summaryText;
    [SerializeField] private TMP_Text _finalPortfolioValueText;
    [SerializeField] private TMP_Text _finalPercentageProfitText;
    [SerializeField] private TMP_Text _bestInvestmentText;
    [SerializeField] private TMP_Text _worstInvestmentText;

    [SerializeField] private Transform _historyContent;
    [SerializeField] private GameObject _historyContentPrefab;
    [SerializeField] private Button _startNewGameButton;

    private void OnEnable()
    {
        GameEvents.OnGameEnded += ShowSummaryPanel;
        _startNewGameButton.onClick.AddListener(() =>
        {
            _summaryPanel.SetActive(false);
            GameEvents.RaiseOnStartNewGame();
        });
    }
    private void OnDisable()
    {
        GameEvents.OnGameEnded -= ShowSummaryPanel;
        _startNewGameButton.onClick.RemoveAllListeners();
    }

    private void ShowSummaryPanel()
    {
        _summaryPanel.SetActive(true);
        summaryText.text = "Koniec gry!";
        _finalPortfolioValueText.text = FormatPortfolioValue();
        UpdateFinalPercentageProfitText();

        var (bestStock, bestProfit, worstStock, worstProfit) = InvestmentAnalyzer.Instance.CalculateBestAndWorstInvestmentReturn();
        UpdateBestAndWorstInvestments(bestStock, bestProfit, worstStock, worstProfit);

        PopulateHistory();
    }

    private string FormatPortfolioValue()
    {
        float value = _playerPortfolio.PlayerCash + _playerPortfolio.GetTotalStockValue();
        return $"Wartość portfela: ${value:F2}";
    }

    private void UpdateFinalPercentageProfitText()
    {
        float finalPercentageProfit = _playerPortfolio.GetProfitPercentage();
        SetFinalPercentageProfitText(finalPercentageProfit);
    }

    public void UpdateBestAndWorstInvestments(Stock bestStock, float bestProfit, Stock worstStock, float worstProfit)
    {
        _bestInvestmentText.text = FormatInvestmentText("Najlepsza inwestycja", bestStock, bestProfit);
        _worstInvestmentText.text = FormatInvestmentText("Najgorsza inwestycja", worstStock, worstProfit);
    }

    private string FormatInvestmentText(string label, Stock stock, float profit)
    {
        string stockName = stock != null ? stock.stockData.stockName : "-";
        return $"{label}: {stockName}: Zysk: ${profit:F2}";
    }

    private void PopulateHistory()
    {
        foreach (var entry in _gameHistory.GetEntries())
        {
            var go = Instantiate(_historyContentPrefab, _historyContent);
            go.GetComponentInChildren<TMP_Text>().text = entry;
        }
    }

    private void SetFinalPercentageProfitText(float value)
    {
        if (value > 0)
        {
            _finalPercentageProfitText.color = Color.green;
            _finalPercentageProfitText.text = $"Zysk procentowy: +{value:F2}%";
        }
        else if (value < 0)
        {
            _finalPercentageProfitText.color = Color.red;
            _finalPercentageProfitText.text = $"Strata procentowa: {value:F2}%";
        }
        else
        {
            _finalPercentageProfitText.color = Color.white;
        }
    }
}
