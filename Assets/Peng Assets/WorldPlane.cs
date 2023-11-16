using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlane : MonoBehaviour
{
    [SerializeField] private GameObject BackgroundCellTemplate;
    [SerializeField] private GameObject PlaceableCellTemplate;

    [SerializeField] public GameObject testPrefabA;
    
    [SerializeField] private int startingLevel;
    private int level = 0;

    private Dictionary<Vector2Int, GameObject> AllCells = new Dictionary<Vector2Int, GameObject>();

    enum PlaneState
    {
        Waiting, Generating
    }

    private PlaneState planeState = PlaneState.Waiting;

    private void Start()
    {
        GenerateLevel(3,true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateLevel(1);
        }
    }

    public void GenerateLevel(int amount, bool placeable = false)
    {
        GameObject goToGenerate = placeable ? PlaceableCellTemplate : BackgroundCellTemplate;
        while (amount != 0)
        {
            amount--;
            
            for (int i = -level; i <= level; i++)
            {
                for (int j = -level; j <= level; j++)
                {
                    if (i == -level || i == level || j == -level || j == level)
                    {
                        GameObject newCell = Instantiate(goToGenerate);
                        newCell.SetActive(true);
                        newCell.transform.position = new Vector3(i, j, 0);
                    }
                }
            }

            level++;
        }
    }
}
