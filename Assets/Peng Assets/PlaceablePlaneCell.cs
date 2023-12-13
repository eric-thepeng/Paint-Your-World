using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceablePlaneCell : PlaneCell
{
    public bool canPlace = true;
    private void OnMouseUp()
    {
        if(!canPlace) return;
        AudioManager.instance.Play("Place Down");
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // zero z

        PlaceableIdentifier targetPI = InventoryManager.i.GetSelectedPlaceableIdentifier();
        GameObject newGO = Instantiate(targetPI.prefab,this.transform);
        newGO.transform.position = mouseWorldPos;
        newGO.transform.localScale = GenerationManager.i.placeableScale * targetPI.defaultScale;

        cellStats.AddPlaceableIdentifier(InventoryManager.i.GetSelectedPlaceableIdentifier());

        CreatureController potCreatureController = newGO.GetComponent<CreatureController>();
        if(potCreatureController != null) potCreatureController.creatureMan = parentWorldPlane.GetCreatureManager();
    }
}
