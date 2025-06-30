using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text cashText;
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private Button nextTurnButton;

    private void OnEnable()
    {
        GameEvents.OnCashChanged += UpdateCashUI;
        GameEvents.OnTurnEnded += UpdateTurnUI;

        nextTurnButton.onClick.AddListener(() => GameEvents.RaiseOnNextTurnRequested());
    }

    private void OnDisable()
    {
        GameEvents.OnCashChanged -= UpdateCashUI;
        GameEvents.OnTurnEnded -= UpdateTurnUI;

        nextTurnButton.onClick.RemoveListener(() => GameEvents.RaiseOnNextTurnRequested());
    }

    private void UpdateCashUI(float newCash)
    {
        cashText.text = $"Cash: ${newCash:F2}";
    }

    private void UpdateTurnUI(int turn)
    {
        turnText.text = $"Turn: {turn + 1}";
    }
}
