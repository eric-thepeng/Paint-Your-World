using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteChanger : MonoBehaviour
{
    private GameObject lastClickedItem = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (var hit in results)
            {
                if (hit.gameObject != null && hit.gameObject.CompareTag("inventory"))
                {
                    GameObject clickedItem = hit.gameObject;

                    if (lastClickedItem != null)
                    {
                        // Reset the previous clicked item
                        lastClickedItem.GetComponent<SpriteHolder>().ResetToOriginalSprite();
                    }

                    // Change the sprite of the new clicked item
                    clickedItem.GetComponent<SpriteHolder>().ChangeToAlternateSprite();
                    lastClickedItem = clickedItem;
                    break; // Break the loop once the inventory item is found and processed
                }
            }
        }
    }
}

