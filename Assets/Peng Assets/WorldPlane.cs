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

    private Dictionary<Vector2Int, PlaneCell> allCells = new Dictionary<Vector2Int, PlaneCell>();

    public class Adjacency
    {
        public List<CellStats> statsPair;
        public int weight;

        public Adjacency(CellStats cellStats1, CellStats cellStats2)
        {
            statsPair = new List<CellStats>(){cellStats1, cellStats2};
            weight = 1;
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
        allAdjacncies = new List<Adjacency>();
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
                        allCells.Add(new Vector2Int(i,j),newCell.GetComponent<PlaneCell>());
                    }
                }
            }
            level++;
        }
    }

    public void AnalyzeAdjacency()
    {
        foreach (KeyValuePair<Vector2Int, PlaneCell> cell in allCells)
        {
            print(cell.Value.cellStats.amount);
        }

        Vector2Int[] directions = new Vector2Int[4]
            { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        
        foreach (var kvp in allCells)
        {
            foreach (var dir in directions)
            {
                if(!allCells.ContainsKey(kvp.Key + dir)) continue;
                List<CellStats> statsPair = new List<CellStats>() {kvp.Value.cellStats, allCells[kvp.Key + dir].cellStats};
                ProcessAdjacency(statsPair);
            }
        }

        foreach (Adjacency adjacency in allAdjacncies)
        {
            adjacency.weight /= 2;
            print("adj " + adjacency.statsPair[0].amount + " - " + adjacency.statsPair[1].amount + " weight: " + adjacency.weight);
        }
        
    }

    private void ProcessAdjacency(List<CellStats> statsPair)
    {
        if(statsPair.Count!=2) return;
        foreach (var adjacency in allAdjacncies)
        {
            if ((adjacency.statsPair[0].Equals(statsPair[0]) && adjacency.statsPair[1].Equals(statsPair[1])) ||
                (adjacency.statsPair[0].Equals(statsPair[1]) && adjacency.statsPair[1].Equals(statsPair[0])))
            {
                adjacency.weight++;
                return;
            }
        }

        Adjacency newAdjacency = new Adjacency(statsPair[0], statsPair[1]);
        allAdjacncies.Add(newAdjacency);
    }
}
