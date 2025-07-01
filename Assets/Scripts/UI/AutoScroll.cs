using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    private ScrollRect _scrollRect;
    [SerializeField] private float _scrollDelay = 0.05f;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        GameHistory.OnEntryAdded += ScrollToBottom;
    }

    private void OnDisable()
    {
        GameHistory.OnEntryAdded -= ScrollToBottom;
    }

    private void ScrollToBottom()
    {
        StartCoroutine(ScrollToBottomCoroutine());
    }

    private IEnumerator ScrollToBottomCoroutine()
    {
        yield return new WaitForSeconds(_scrollDelay);
        _scrollRect.verticalNormalizedPosition = 0f;
    }
}
