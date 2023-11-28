using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceablePlaneCell : PlaneCell
{
    private void OnMouseUp()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // zero z
        
        GameObject newGO = Instantiate(GenerationManager.i.placingPrefab,this.transform);
        newGO.transform.position = mouseWorldPos;

        cellStats.amount++;
    }
}
