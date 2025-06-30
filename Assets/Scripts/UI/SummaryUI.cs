using TMPro;
using UnityEngine;

public class SummaryUI : MonoBehaviour
{
    [SerializeField] private GameHistory gameHistory;
    [SerializeField] private GameObject summaryPanel;
    [SerializeField] private TMP_Text summaryText;
    [SerializeField] private Transform historyContent;
    [SerializeField] private GameObject historyItemPrefab;

    private void OnEnable()
    {
        GameEvents.OnGameEnded += ShowSummaryPanel;
    }
    private void OnDisable()
    {
        GameEvents.OnGameEnded -= ShowSummaryPanel;
    }

    private void ShowSummaryPanel()
    {
        summaryPanel.SetActive(true);
        summaryText.text = "Game Over!\n";

        foreach (var entry in gameHistory.GetEntries())
        {
            var go = Instantiate(historyItemPrefab, historyContent);
            go.GetComponentInChildren<TMP_Text>().text = entry;
        }
    }
}
