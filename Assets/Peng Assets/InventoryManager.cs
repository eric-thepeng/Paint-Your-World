using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    static InventoryManager instance;
    public static InventoryManager i
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<InventoryManager>();
            }
            return instance;
        }
    }
    
    [SerializeField] private GameObject inventoryItemTemplate;
    [SerializeField] private Vector3 inventoryItemDisplacement;

    private List<InventoryItem> allInventoryItems;
    private bool inDrawingMode = false;

    public int selectedInventoryItemIndex = 0;

    public PlaceableIdentifier currentPlaceableIdentifier = null;
    public InventoryItem currentInventoryItem = null;

    public PlaceableIdentifier GetSelectedPlaceableIdentifier()
    {
        return currentPlaceableIdentifier;
    }
    

    private void EnterDrawingMode()
    {
        /*
        if(inDrawingMode)return;
        inDrawingMode = true;
        allInventoryItems = new List<InventoryItem>();
        for (int i = 0; i < allPlaceableIdentifier.Count; i++)
        {
            GameObject newII = Instantiate(inventoryItemTemplate, this.transform);
            newII.SetActive(true);
            newII.GetComponent<RectTransform>().position += inventoryItemDisplacement * i;
            allInventoryItems.Add(new InventoryItem(newII, allPlaceableIdentifier[i]));
        }
        inventoryItemTemplate.SetActive(false);*/
    }

    public void SelectInventoryItem(InventoryItem ii)
    {
        currentInventoryItem = ii;
        currentPlaceableIdentifier = ii.placeableIdentifier;
    }
    
    public void FinishPaintingCurrent()
    {
        currentInventoryItem.FinishPainting();
    }

    
}
