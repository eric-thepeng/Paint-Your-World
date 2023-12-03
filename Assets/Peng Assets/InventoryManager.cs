using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    
    public class InventoryItem
    {
        public GameObject gameObject;
        public TMP_Text nameText;
        public Image placeableIdentifierImage;
        public PlaceableIdentifier placeableIdentifier;

        public InventoryItem(GameObject gameObject, PlaceableIdentifier placeableIdentifier)
        {
            this.gameObject = gameObject;
            this.placeableIdentifier = placeableIdentifier;
            nameText = gameObject.transform.Find("Name").GetComponent<TMP_Text>();
            placeableIdentifierImage = gameObject.transform.Find("Sprite").GetComponent<Image>();
            nameText.text = placeableIdentifier.name;
        }

        public void FinishPainting()
        {
            placeableIdentifierImage.enabled = true;
            placeableIdentifierImage.sprite = placeableIdentifier.prefab.GetComponent<SpriteRenderer>().sprite;
        }

    }
    
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
    [SerializeField] private List<PlaceableIdentifier> allPlaceableIdentifier;
    [SerializeField] private Vector3 inventoryItemDisplacement;

    private List<InventoryItem> allInventoryItems;
    private bool inDrawingMode = false;

    public int selectedInventoryItemIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            EnterDrawingMode();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedInventoryItemIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedInventoryItemIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedInventoryItemIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedInventoryItemIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedInventoryItemIndex = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedInventoryItemIndex = 5;
        }

    }

    public PlaceableIdentifier GetSelectedPlaceableIdentifier()
    {
        return allPlaceableIdentifier[selectedInventoryItemIndex];
    }

    public void FinishPaintingPlaceableIdentifier(PlaceableIdentifier tarPlaceableIdentifier)
    {
        foreach (var VARIABLE in allInventoryItems)
        {
            if (VARIABLE.placeableIdentifier == tarPlaceableIdentifier)
            {
                VARIABLE.FinishPainting();
            }
        }
    }

    private void EnterDrawingMode()
    {
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
        inventoryItemTemplate.SetActive(false);
    }
}
