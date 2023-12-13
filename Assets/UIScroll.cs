using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScroll : MonoBehaviour
{
    [Tooltip("Always positive")]
    /*
    public float scrollAmount=.3f;
    public int totalItems = 8; // Total number of items in the scroll view
    public int visibleItems = 3; // Number of items visible at a time
    */
    public float lerpTime = 0.1f; // Time taken to lerp to the new position

    //private RectTransform m_RectTransform;
    //private float itemHeight;
    public float targetYPosition;
    //private int currentIndex = 0;

    bool isScrolling = false;


    public enum Direction
    {
        Up,
        Down
    }

    void Start()
    {
        //m_RectTransform = GetComponent<RectTransform>();
        //itemHeight = m_RectTransform.rect.height / visibleItems; // Assuming equal height for all items
        //scrollAmount = itemHeight; // Update scrollAmount to be equal to one item's height
        //targetYPosition = m_RectTransform.anchoredPosition.y;
        targetYPosition = transform.parent.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Scroll(Direction.Down);
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Scroll(Direction.Up);
        //}

        // Lerp to the target position

        //Vector2 newPosition = m_RectTransform.anchoredPosition;
        //newPosition.y = Mathf.Lerp(newPosition.y, targetYPosition, lerpTime);
        //m_RectTransform.anchoredPosition = newPosition;


        //J-New scroll system to be compatible with scrollrect
        if (isScrolling)
        {
            float newPosition = transform.parent.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition;
            newPosition = Mathf.Lerp(newPosition, targetYPosition, lerpTime);
            transform.parent.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = newPosition;

            if (Mathf.Abs(targetYPosition - transform.parent.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition) <= 0.01f)
            {
                isScrolling = false;
                transform.parent.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = targetYPosition;
            }
        }
    }

    public void Scroll(Direction direction)
    {
        //if (direction == Direction.Up && currentIndex > 0)
        //{
        //    currentIndex--;
        //    UpdateTargetPosition();
        //}
        //else if (direction == Direction.Down && currentIndex < totalItems - visibleItems)
        //{
        //    currentIndex++;
        //    UpdateTargetPosition();
        //}

        if(direction == Direction.Up)
        {
            ScrowUpButton();
        }
        else if (direction == Direction.Down)
        {
            ScrowDownButton();
        }
    }

    //J-For Up Arrow Button
    public void ScrowUpButton()
    {
       isScrolling = true;
       targetYPosition += Mathf.Min(0.3f, 1f-transform.parent.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition);

    }

    //J-For Down Arrow Button
    public void ScrowDownButton()
    {
        isScrolling = true;
        targetYPosition -= Mathf.Min(0.3f,transform.parent.transform.parent.GetComponent<ScrollRect>().verticalNormalizedPosition);
    }

    private void UpdateTargetPosition()
    {
        //targetYPosition = -currentIndex * scrollAmount;
    }
}
