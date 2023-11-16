using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlaneCell : PlaneCell
{

    public void AssignCellStats(CellStats newCellStats)
    {
        cellStats = newCellStats;
        print("cell stats assigned: " + cellStats);

        for (int i = 0; i < cellStats.amount; i++)
        {
            Vector3 treePosition = new Vector3(Random.Range(-0.5f, 0.5f),Random.Range(-0.5f, 0.5f),0);
            GameObject newGO = Instantiate(GenerationManager.i.placingPrefab,this.transform);
            newGO.transform.localPosition = treePosition;
        }

    }

}
