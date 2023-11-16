using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldPlane : MonoBehaviour
{
    [SerializeField] private GameObject BackgroundCellTemplate;
    [SerializeField] private GameObject PlaceableCellTemplate;

    [SerializeField] public GameObject testPrefabA;
    
    [SerializeField] private int startingLevel;
    private int level = 0;

    private Dictionary<Vector2Int, GameObject> allCells = new Dictionary<Vector2Int, GameObject>();

    public class Adjacency
    {
        private List<CellStats> elements;
        private int weight;

        public Adjacency()
        {
            elements = new List<CellStats>();
            weight = 0;
        }
        
    }

    private List<Adjacency> allAdjacncies;

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
        if (Input.GetKeyDown(KeyCode.J))
        {
            GenerateLevel(1);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            AnalyzeAdjacency();
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
                        GameObject newCell = Instantiate(goToGenerate,transform);
                        newCell.SetActive(true);
                        newCell.transform.position = new Vector3(i, j, 0);
                        if(placeable) newCell.GetComponent<PlaceablePlaneCell>().SetUp(this);
                        allCells.Add(new Vector2Int(i,j),newCell);
                    }
                }
            }
            level++;
        }
    }

    public void AnalyzeAdjacency()
    {
        foreach (var VARIABLE in allCells)
        {
            print(VARIABLE.Value.GetComponent<PlaceablePlaneCell>()?.cellStats.amount);
        }

        Vector2Int[] directions = new Vector2Int[4]
            { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        
        foreach (var kvp in allCells)
        {
            foreach (var dir in directions)
            {
                if(!allCells.ContainsKey(kvp.Key + dir)) continue;
                //List<CellStats> statsPair = new List<CellStats>() {kvp.Key };
            }
        }
        
    }
}
