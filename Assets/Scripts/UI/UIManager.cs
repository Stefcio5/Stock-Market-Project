using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _cashText;
    [SerializeField] private TMP_Text _turnText;
    [SerializeField] private Button _nextTurnButton;

    private void OnEnable()
    {
        GameEvents.OnCashChanged += UpdateCashUI;
        GameEvents.OnTurnEnded += UpdateTurnUI;

        _nextTurnButton.onClick.AddListener(() => GameEvents.RaiseOnNextTurnRequested());
    }

    private void OnDisable()
    {
        GameEvents.OnCashChanged -= UpdateCashUI;
        GameEvents.OnTurnEnded -= UpdateTurnUI;

        _nextTurnButton.onClick.RemoveListener(() => GameEvents.RaiseOnNextTurnRequested());
    }

    private void UpdateCashUI(float cash)
    {
        _cashText.text = $"Stan konta: ${cash:F2}";
    }

    private void UpdateTurnUI(int turn)
    {
        _turnText.text = $"Tura: {turn + 1}";
    }
}
