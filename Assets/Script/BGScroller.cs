using UnityEngine;
using UnityEngine.UI;

public class BGScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 200f; // Speed of the background movement
    private RectTransform rectTransform;
    private float resetHeight;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Get the height of the background image to know when to reset
        resetHeight = rectTransform.rect.height;
    }

    private void Update()
    {
        // 1. Move the background downward continuously
        rectTransform.anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;

        // 2. Reset position when it moves down by its full height to create a seamless loop
        if (rectTransform.anchoredPosition.y <= -resetHeight)
        {
            // Snap back up by the reset height to loop endlessly without a stutter
            rectTransform.anchoredPosition += new Vector2(0f, resetHeight * 2f);
        }
    }
}