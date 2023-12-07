using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    //public TMP_Text nameText;
    public Image placeableIdentifierImage;
    public PlaceableIdentifier placeableIdentifier;

    private void Awake()
    {
        //placeableIdentifierImage = gameObject.transform.Find("Sprite").GetComponent<Image>();
    }

    public InventoryItem(GameObject gameObject, PlaceableIdentifier placeableIdentifier)
    {
        //this.gameObject = gameObject;
        //this.placeableIdentifier = placeableIdentifier;
        //nameText = gameObject.transform.Find("Name").GetComponent<TMP_Text>();
        //placeableIdentifierImage = gameObject.transform.Find("Sprite").GetComponent<Image>();
        //nameText.text = placeableIdentifier.name;
    }

    public void FinishPainting()
    {
        placeableIdentifierImage.gameObject.SetActive(true);
        placeableIdentifierImage.sprite = placeableIdentifier.prefab.GetComponent<SpriteRenderer>().sprite;
    }
    

}