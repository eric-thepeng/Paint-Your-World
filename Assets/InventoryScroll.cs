using UnityEngine;
using UnityEngine.UI;

public class InventoryScroll : MonoBehaviour
{
    public GameObject inventoryContainer;
    public float boxHeight;
    public int totalBoxes = 8;
    public float scrollSpeed = 5f;

    private int currentIndex = 0;
    private float targetYPosition;

    void Start()
    {
        targetYPosition = inventoryContainer.transform.localPosition.y;
    }

    public void ScrollUp()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateTargetPosition();
        }
    }

    public void ScrollDown()
    {
        if (currentIndex < totalBoxes - 3) // Assuming each 'page' shows 3 boxes
        {
            currentIndex++;
            UpdateTargetPosition();
        }
    }

    void UpdateTargetPosition()
    {
        targetYPosition = -currentIndex * boxHeight;
    }

    void Update()
    {
        Vector3 newPosition = inventoryContainer.transform.localPosition;
        newPosition.y = Mathf.Lerp(newPosition.y, targetYPosition, Time.deltaTime * scrollSpeed);
        inventoryContainer.transform.localPosition = newPosition;
    }
}
