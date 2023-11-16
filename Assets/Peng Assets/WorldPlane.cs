using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldPlane : MonoBehaviour
{
    [SerializeField] private GameObject BackgroundCellTemplate;
    [SerializeField] private GameObject PlaceableCellTemplate;

    [SerializeField] public GameObject testPrefabA;
    
    [SerializeField] private int startingLevel;
    private int level = 0;

    [SerializeField]private float unitSize = 1f;

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
                        // Create new cell
                        GameObject newCell = Instantiate(goToGenerate,transform);
                        newCell.SetActive(true);
                        newCell.transform.position = new Vector3(i * unitSize, j* unitSize, 0);
                        allCells.Add(new Vector2Int(i,j),newCell.GetComponent<PlaneCell>());
                        
                        // Populate with placeables - Placable Cell
                        if(placeable) newCell.GetComponent<PlaceablePlaneCell>().SetUp(this);

                        // Populate with placeables - Background Cell
                        else
                        {
                            newCell.GetComponent<BackgroundPlaneCell>()
                                .AssignCellStats(GenerateNewCellStats(new Vector2Int(i, j)));
                        }
                        
                        // Adujust Size 
                        newCell.transform.localScale = new Vector3(unitSize, unitSize, unitSize);
                    }
                }
            }
            level++;
        }
    }

    public CellStats GenerateNewCellStats(Vector2Int coord)
    {
        //eliminate error cases
        if (!allCells.ContainsKey(coord))Debug.LogError("Does not contain this coordination to generate new CellStats");
        if(allCells[coord].cellStats != null)Debug.LogError("This coordination already has a CellStats");

        //populate and set up
        allCells[coord].cellStats = new CellStats();
        List<Dictionary<CellStats, int>> possibilitiesList = new List<Dictionary<CellStats, int>>();

        //calculate neighbour
        Vector2Int[] adjCoords = new Vector2Int[4]
        {
            coord + new Vector2Int(1, 0), 
            coord + new Vector2Int(-1, 0), 
            coord + new Vector2Int(0, 1),
            coord + new Vector2Int(0, -1)
        };
        
        foreach (Vector2Int coordToCheck in adjCoords) //each neighbor cell
        {
            if(!allCells.ContainsKey(coordToCheck)) continue;
            if(allCells[coordToCheck].cellStats == null) continue;

            Dictionary<CellStats, int> possibilitiesForThisNeighbor = new Dictionary<CellStats, int>();

            CellStats adjCellStats = allCells[coordToCheck].cellStats; //each adjacency
            foreach (Adjacency adjacency in allAdjacncies)
            {
                CellStats csToAdd = null;
                if (adjacency.statsPair[0].Equals(adjCellStats))
                {
                    csToAdd = adjacency.statsPair[1];
                }else if (adjacency.statsPair[1].Equals(adjCellStats))
                {
                    csToAdd = adjacency.statsPair[0];
                }
                if(csToAdd == null) continue;
                
                possibilitiesForThisNeighbor.Add(csToAdd,adjacency.weight);
            }
            
            possibilitiesList.Add(possibilitiesForThisNeighbor);
        }

        //find overlaps between possibilities
        Dictionary<CellStats, int> overlapPossibilities = new Dictionary<CellStats, int>();

        foreach (var neighborPossibility in possibilitiesList)
        {
            foreach (var kvp in neighborPossibility)
            {
                bool toAdd = true;
                foreach (var neighborPossibilityAgain in possibilitiesList)
                {
                    if (!neighborPossibilityAgain.ContainsKey(kvp.Key)) toAdd = false;
                }

                if (toAdd)
                {
                    if (overlapPossibilities.ContainsKey(kvp.Key)) overlapPossibilities[kvp.Key] += kvp.Value;
                    else overlapPossibilities.Add(kvp.Key,kvp.Value);
                }

            }
        }
        
        //Collapsing and Assigning
        CellStats finalCellStats = null;
        if (overlapPossibilities.Count == 0) finalCellStats = new CellStats();
        else
        {
            int totalWeight = 0;
            foreach (var kvp in overlapPossibilities)
            {
                totalWeight += kvp.Value;
            }

            int weight = Random.Range(0, totalWeight);

            foreach (var kvp in overlapPossibilities)
            {
                if (kvp.Value >= weight)
                {
                    finalCellStats = kvp.Key;
                    return finalCellStats;
                }
                else
                {
                    weight -= kvp.Value;
                }
            }
        }
        if(finalCellStats == null) Debug.LogError("Final Cell Stats is null");
        return finalCellStats;
    }

    public void AnalyzeAdjacency()
    {
        //populate and print allPossibilities
        foreach (KeyValuePair<Vector2Int, PlaneCell> cell in allCells)
        {
            print(cell.Value.cellStats.amount);
            if (!CellStats.allPossibilities.Contains(cell.Value.cellStats))
            {
                CellStats.allPossibilities.Add(cell.Value.cellStats);
            }
        }

        //check each direction and analyze adjacency scheme
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

        //print each adjacency
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
