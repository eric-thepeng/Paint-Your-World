using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScroll : MonoBehaviour
{
    [Tooltip("Always positive")]
    public float scrollAmount=.3f;
    public int totalItems = 8; // Total number of items in the scroll view
    public int visibleItems = 3; // Number of items visible at a time
    public float lerpTime = 0.1f; // Time taken to lerp to the new position

    private RectTransform m_RectTransform;
    private float itemHeight;
    private float targetYPosition;
    private int currentIndex = 0;

    public enum Direction
    {
        Up,
        Down
    }

    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        itemHeight = m_RectTransform.rect.height / visibleItems; // Assuming equal height for all items
        //scrollAmount = itemHeight; // Update scrollAmount to be equal to one item's height
        targetYPosition = m_RectTransform.anchoredPosition.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Scroll(Direction.Down);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Scroll(Direction.Up);
        }

        // Lerp to the target position
        Vector2 newPosition = m_RectTransform.anchoredPosition;
        newPosition.y = Mathf.Lerp(newPosition.y, targetYPosition, lerpTime);
        m_RectTransform.anchoredPosition = newPosition;
    }

    public void Scroll(Direction direction)
    {
        if (direction == Direction.Up && currentIndex > 0)
        {
            currentIndex--;
            UpdateTargetPosition();
        }
        else if (direction == Direction.Down && currentIndex < totalItems - visibleItems)
        {
            currentIndex++;
            UpdateTargetPosition();
        }
    }

    private void UpdateTargetPosition()
    {
        targetYPosition = -currentIndex * scrollAmount;
    }
}
