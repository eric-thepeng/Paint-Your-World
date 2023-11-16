using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlaneCell : PlaneCell
{

    public void AssignCellStats(CellStats newCellStats)
    {
        cellStats = newCellStats;
        print("cell stats assigned: " + cellStats);
    }

}
