using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCell : MonoBehaviour
{
    protected WorldPlane parentWorldPlane;
    public CellStats cellStats = null;
    public SpriteRenderer groundSprite;
    
    public void SetUp(WorldPlane parentWorldPlane)
    {
        this.parentWorldPlane = parentWorldPlane;
        groundSprite.color = parentWorldPlane.basePlaneColor;
        cellStats = new CellStats();
    }
}
