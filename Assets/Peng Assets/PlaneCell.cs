using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCell : MonoBehaviour
{
    protected WorldPlane parentWorldPlane;
    public CellStats cellStats = null;
    
    public void SetUp(WorldPlane parentWorldPlane)
    {
        this.parentWorldPlane = parentWorldPlane;
        cellStats = new CellStats();
    }
}
