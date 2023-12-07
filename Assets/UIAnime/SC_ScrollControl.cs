using UnityEngine;
using UnityEngine.UI;

public class SC_ScrollControl : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollStep = 0.1f; // Adjust this value for larger or smaller steps

    public void ScrollUp()
    {
        scrollRect.verticalNormalizedPosition += scrollStep;
        ClampScrollPosition();
    }

    public void ScrollDown()
    {
        scrollRect.verticalNormalizedPosition -= scrollStep;
        ClampScrollPosition();
    }

    private void ClampScrollPosition()
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition, 0f, 1f);
    }
}
