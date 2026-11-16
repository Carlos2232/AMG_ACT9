using UnityEngine;
using UnityEngine.UI;

public class BGScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 200f; 
    private RectTransform rectTransform;
    private float resetHeight;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        resetHeight = rectTransform.rect.height;
    }

    private void Update()
    {
        rectTransform.anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;

        if (rectTransform.anchoredPosition.y <= -resetHeight)
        {
            rectTransform.anchoredPosition += new Vector2(0f, resetHeight * 2f);
        }
    }
}