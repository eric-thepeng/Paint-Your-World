using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldPlane : MonoBehaviour
{
    [SerializeField] private GameObject BackgroundCellTemplate;
    [SerializeField] private GameObject PlaceableCellTemplate;
    
    [SerializeField] private int startingLevel;
    [SerializeField] private int totalLevel;
    [SerializeField] private int maxTryTime = 30;
    private int level = 0;
    
    [SerializeField]private float unitSize = 1f;

    private Dictionary<Vector2Int, PlaneCell> startingCells;

    private SuperPosition[,] superPositionsGrid;

    Vector2Int[] allDirections = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
    
    public class Adjacency
    {
        public CellStats orgCellStat;
        public CellStats tarCellStat;
        public Vector2Int direction;
        public int weight;

        public Adjacency(CellStats orgCellStat, CellStats tarCellStat, Vector2Int direction)
        {
            this.orgCellStat = orgCellStat;
            this.tarCellStat = tarCellStat;
            weight = 1;
            this.direction = direction;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is not Adjacency) return false;
            Adjacency oppAdjacency = (Adjacency)obj;
            if (this.orgCellStat.Equals(oppAdjacency.orgCellStat) &&
                this.tarCellStat.Equals(oppAdjacency.tarCellStat) &&
                this.direction == oppAdjacency.direction) return true;
            return false;
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
        GenerateStartingPlane(startingLevel);
        allAdjacncies = new List<Adjacency>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GenerateWorld(); //GenerateLevel(1);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            AnalyzeAdjacency();
        }
    }

    public void GenerateStartingPlane(int amount)
    {
        GameObject goToGenerate = PlaceableCellTemplate;
        startingCells = new Dictionary<Vector2Int, PlaneCell>();
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
                        startingCells.Add(new Vector2Int(i + startingLevel,j + startingLevel),newCell.GetComponent<PlaneCell>());
                        
                        // Populate with placeables - Placable Cell
                        newCell.GetComponent<PlaceablePlaneCell>().SetUp(this);
                        
                        // Adujust Size 
                        newCell.transform.localScale = new Vector3(unitSize, unitSize, unitSize);
                    }
                }
            }
            level++;
        }
    }

    public void GenerateWorld()
    {
        int currenTryTime = 0;
        bool generationSuccess = false;
        do
        {
            currenTryTime += 1;
            if (PerformWFC())
            {
                generationSuccess = true;
                Debug.Log("WFC SUCCESS IN " + currenTryTime + " RUNS.");
                BuildTheWorld();
                return;
            }
        }
        while (currenTryTime <= maxTryTime && generationSuccess == false);
        
        Debug.Log("Cannot find WFC solution after " + maxTryTime + " tries.");
    }

    void BuildTheWorld()
    {
        for (int x = 0; x < 2*totalLevel - 1; x++)
        {
            for (int y = 0; y < 2*totalLevel - 1; y++)
            {
                if (!startingCells.ContainsKey(new Vector2Int(x, y)))
                {
                    // Create new cell
                    GameObject newCell = Instantiate(BackgroundCellTemplate,transform);
                    newCell.SetActive(true);
                    newCell.transform.position = new Vector3((x-startingLevel) * (unitSize), (y-startingLevel)* (unitSize), 0);
                    newCell.GetComponent<BackgroundPlaneCell>()
                        .AssignCellStats(superPositionsGrid[x,y].GetObservedValue());
                    
                    // Adujust Size 
                    newCell.transform.localScale = new Vector3(unitSize, unitSize, unitSize);
                }
            }
        }
    }

    public bool PerformWFC()
    {
        InitializeGrid();
        
        //STAGE 1 Deal With Boarder Cells
        
        
        //STAGE 2 Deal With Rest
        while (DoUnobservedNodesExist())
        {
            Vector2Int node = GetNextUnobservedNode();
            
            if (!superPositionsGrid[node.x,node.y].HasPossibilities())
            {
                return false; //failure
            }
            
            superPositionsGrid[node.x, node.y].Observe();
            PropogateNeighbors(node);
        }
        return true;
    }
    
    bool DoUnobservedNodesExist()
    {
        for (int x = 0; x < 2*totalLevel - 1; x++)
        {
            for (int y = 0; y < 2*totalLevel - 1; y++)
            {
                if (superPositionsGrid[x, y].IsObserved() == false) {
                    return true;
                }
            }
        }
        return false;
    }
    
    Vector2Int GetNextUnobservedNode()
    {
        int minPossibleAmount = 2147483646;
        Vector2Int minPossibleCoord = new Vector2Int(0, 0);
        for (int x = 0; x < 2*totalLevel - 1; x++)
        {
            for (int y = 0; y < 2*totalLevel - 1; y++)
            {
                if (!(superPositionsGrid[x, y].IsObserved()) && superPositionsGrid[x, y].NumOptions < minPossibleAmount)
                {
                    minPossibleAmount = superPositionsGrid[x, y].NumOptions;
                    minPossibleCoord = new Vector2Int(x, y);
                }
            }
        }
        
        //return the coordinates of the unobserved node with the fewest possible options
        return minPossibleCoord; 
    }
    
    void PropogateTo(Vector2Int node, Vector2Int direction)
    {
        Vector2Int tarNode = node + direction;
        
        if(tarNode.x < 0 || tarNode.x >= 2*totalLevel-1 || tarNode.y < 0 || tarNode.y >= 2*totalLevel-1) return;
        SuperPosition spToCheck = superPositionsGrid[tarNode.x, tarNode.y];
        if (spToCheck.IsObserved()) return;

        for(int i = spToCheck.RemainPossibleValues().Count-1; i >=0 ; i--)
        {
            bool keep = false;
            foreach (var adjacency in allAdjacncies)
            {
                if (adjacency.orgCellStat.Equals(superPositionsGrid[node.x, node.y].GetObservedValue()) 
                    && adjacency.tarCellStat.Equals(spToCheck.RemainPossibleValues()[i]) 
                    && adjacency.direction == direction)
                {
                    keep = true;
                }
            }
            if(!keep)spToCheck.RemovePossibleValue(spToCheck.RemainPossibleValues()[i]);
        }
        
    }

    void PropogateNeighbors(Vector2Int node)
    {
        SuperPosition sp = superPositionsGrid[node.x, node.y];

        if(!sp.IsObserved()) Debug.LogError("SuperPosition not observed");
        foreach (var dir in allDirections)
        {
            PropogateTo(node,dir);
        }
    }
    

    public void InitializeGrid()
    {
        int gridWidth = 2 * totalLevel - 1;
        superPositionsGrid = new SuperPosition[gridWidth, gridWidth];
        Vector2Int botLeftStartingCell = new Vector2Int(totalLevel - startingLevel, totalLevel - startingLevel);

        //populate superposition grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridWidth; y++)
            {
                superPositionsGrid[x, y] = new SuperPosition(CellStats.allPossibilities);
            }
        }

        //assign exisiting placeable plane
        foreach (var pair in startingCells)
        {
            superPositionsGrid[botLeftStartingCell.x + pair.Key.x, botLeftStartingCell.y + pair.Key.y].AssignObservation(pair.Value.cellStats);
        }
    }

    public List<Vector2Int> GetPlaceableCoord(int placeableLevel, int totalLevel)
    {
        List<Vector2Int> output = new List<Vector2Int>();
        
        int gridWidth = 2 * this.totalLevel;
        Vector2Int botLeftStartingCell = new Vector2Int(totalLevel - startingLevel, totalLevel - startingLevel);
        Vector2Int topRightStartingCell = 2 * new Vector2Int(placeableLevel, placeableLevel) + botLeftStartingCell;

        //populate superposition grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridWidth; y++)
            {
                if ((botLeftStartingCell.x <= x && x <= topRightStartingCell.x) &&
                    (botLeftStartingCell.y <= y && y <= topRightStartingCell.y))
                {
                    output.Add(new Vector2Int(x,y));
                }
            }
        }
        return output;
    }

    /*
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
                        startingCells.Add(new Vector2Int(i,j),newCell.GetComponent<PlaneCell>());
                        
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
        if (!startingCells.ContainsKey(coord))Debug.LogError("Does not contain this coordination to generate new CellStats");
        if(startingCells[coord].cellStats != null)Debug.LogError("This coordination already has a CellStats");

        //populate and set up
        startingCells[coord].cellStats = new CellStats();
        List<Dictionary<CellStats, int>> possibilitiesList = new List<Dictionary<CellStats, int>>();

        //calculate neighbour
        Vector2Int[] adjCoords = new Vector2Int[4]
        {
            coord + new Vector2Int(1, 0), 
            coord + new Vector2Int(-1, 0), 
            coord + new Vector2Int(0, 1),
            coord + new Vector2Int(0, -1)
        };
        
        /*
        foreach (Vector2Int coordToCheck in adjCoords) //each neighbor cell
        {
            if(!startingCells.ContainsKey(coordToCheck)) continue;
            if(startingCells[coordToCheck].cellStats == null) { continue; }

            Dictionary<CellStats, int> possibilitiesForThisNeighbor = new Dictionary<CellStats, int>();

            CellStats adjCellStats = startingCells[coordToCheck].cellStats; //each adjacency
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
    }*/

    public void AnalyzeAdjacency()
    {
        //Reset allPossibilities
        CellStats.allPossibilities = new List<CellStats>();
        allAdjacncies = new List<Adjacency>();
        
        //Populate and print allPossibilities
        foreach (KeyValuePair<Vector2Int, PlaneCell> cell in startingCells)
        {
            print(cell.Value.cellStats);
            if (!CellStats.allPossibilities.Contains(cell.Value.cellStats))
            {
                CellStats.allPossibilities.Add(cell.Value.cellStats);
            }
        }
        
        //Process each pair and direction
        foreach (var kvp in startingCells)
        {
            foreach (var dir in allDirections)
            {
                if (startingCells.ContainsKey(kvp.Key + dir))
                {
                    ProcessAdjacency(kvp.Value.cellStats, startingCells[kvp.Key + dir].cellStats,dir);
                }
                else
                {
                    ProcessAdjacency(kvp.Value.cellStats, startingCells[kvp.Key - dir * (2*startingLevel-2)].cellStats,dir);
                }
            }
        }

        //Print each adjacency
        foreach (Adjacency adjacency in allAdjacncies)
        {
            print("org: " + adjacency.orgCellStat + " tar: " + adjacency.tarCellStat + " dir: " + adjacency.direction.x + " " +adjacency.direction.y + " weight: " + adjacency.weight);
        }
        
    }

    private void ProcessAdjacency(CellStats orgCellStat, CellStats tarCellStat, Vector2Int direction)
    {
        Adjacency tempAdjacency = new Adjacency(orgCellStat, tarCellStat, direction);

        foreach (var adjacency in allAdjacncies)
        {
            // avoid duplicated adjacency add in direction
            if (adjacency.Equals(tempAdjacency))
            {
                adjacency.weight++;
                return;
            }
 
        }
        allAdjacncies.Add(tempAdjacency);
    }
    
}
