using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScroll : MonoBehaviour
{
    [Tooltip("Always positive")]
    public float scrollAmont;

    private RectTransform m_RectTransform;

    public enum Direction
    {
        Up,
        Down
    }

    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            Scroll(Direction.Down);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Scroll(Direction.Up);
        }
    }

    public void Scroll(Direction direction)
    {
        if(direction == Direction.Up)
        {
            m_RectTransform.anchoredPosition = new Vector3(m_RectTransform.anchoredPosition.x, m_RectTransform.anchoredPosition.y + scrollAmont, 0);
        }
        else if(direction == Direction.Down)
        {
            m_RectTransform.anchoredPosition = new Vector3(m_RectTransform.anchoredPosition.x, m_RectTransform.anchoredPosition.y - scrollAmont, 0);
        }
    }
}
