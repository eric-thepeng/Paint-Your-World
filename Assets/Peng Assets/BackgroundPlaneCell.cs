using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlaneCell : PlaneCell
{

    public void AssignCellStats(CellStats newCellStats)
    {
        cellStats = newCellStats;
        print("cell stats assigned: " + cellStats);

        foreach (var kvp in cellStats.stats)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                Vector3 treePosition = new Vector3(Random.Range(-0.5f, 0.5f),Random.Range(-0.5f, 0.5f),0);
                GameObject newGO = Instantiate(kvp.Key.prefab,this.transform);
                newGO.transform.localPosition = treePosition;
                newGO.transform.localScale = GenerationManager.i.placeableScale;
            }
        }
    }

}
