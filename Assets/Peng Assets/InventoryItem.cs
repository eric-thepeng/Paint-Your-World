using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public TMP_Text nameText;
    public Image placeableIdentifierImage;
    public PlaceableIdentifier placeableIdentifier;

    public void FinishPainting()
    {
        placeableIdentifierImage.gameObject.SetActive(true);
        placeableIdentifierImage.sprite = placeableIdentifier.prefab.GetComponent<SpriteRenderer>().sprite;
        //nameText.text = placeableIdentifier.placeableName;
    }

    private void Start()
    {
        nameText.text = placeableIdentifier.placeableName;

    }




}