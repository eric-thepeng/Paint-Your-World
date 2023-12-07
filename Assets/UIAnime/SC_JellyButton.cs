using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SC_JellyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleFactor = 1.2f;
    public float animationSpeed = 3f;

    private Vector3 originalScale;
    private bool isPressed = false;
    private bool isHovering = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isPressed || isHovering)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * scaleFactor, Time.deltaTime * animationSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * animationSpeed);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
