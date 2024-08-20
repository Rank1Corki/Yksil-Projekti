using UnityEngine;
using UnityEngine.UI;

public class InventoryScroll : MonoBehaviour
{
    public ScrollRect scrollRect;  // Reference to the Scroll Rect component

    void Update()
    {
        // Get the mouse scroll delta (scroll wheel input)
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollRect != null && Mathf.Abs(scrollDelta) > 0.01f)
        {
            // Adjust the vertical scroll position based on scroll wheel input
            if (scrollRect.verticalScrollbar != null)
            {
                float newValue = scrollRect.verticalScrollbar.value + scrollDelta;
                scrollRect.verticalScrollbar.value = Mathf.Clamp01(newValue);
            }

            // Optionally adjust the horizontal scrollbar if you use horizontal scrolling
            // if (scrollRect.horizontalScrollbar != null)
            // {
            //     float newValueHorizontal = scrollRect.horizontalScrollbar.value + scrollDelta;
            //     scrollRect.horizontalScrollbar.value = Mathf.Clamp01(newValueHorizontal);
            // }
        }
    }
}
