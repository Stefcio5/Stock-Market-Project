using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    private ScrollRect scrollRect;
    [SerializeField] private float scrollDelay = 0.05f;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        GameHistory.OnEntryAdded += ScrollToBottom;
    }

    private void OnDisable()
    {
        GameHistory.OnEntryAdded -= ScrollToBottom;
    }

    public void ScrollToBottom()
    {
        StartCoroutine(ScrollToBottomCoroutine());
    }

    private IEnumerator ScrollToBottomCoroutine()
    {
        yield return new WaitForSeconds(scrollDelay);
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
