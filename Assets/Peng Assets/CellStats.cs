using System.Collections.Generic;

public class CellStats
{
    //public int amount;
    public static List<CellStats> allPossibilities = new List<CellStats>();

    public Dictionary<PlaceableIdentifier, int> stats;

    public CellStats()
    {
        stats = new Dictionary<PlaceableIdentifier, int>();
    }

    public void AddPlaceableIdentifier(PlaceableIdentifier newPI)
    {
        if (stats.ContainsKey(newPI)) stats[newPI]++;
        else stats.Add(newPI,1);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is CellStats)) return false;
        CellStats otherCS = (CellStats)obj;
        if (stats.Count != otherCS.stats.Count) return false;
        
        foreach (var kvp in stats)
        {
            if (!otherCS.stats.ContainsKey(kvp.Key)) return false;
            if (otherCS.stats[kvp.Key] != kvp.Value) return false;
        }

        return true;
    }

    public override string ToString()
    {
        string toReturn = "CellStats ";

        foreach (var kvp in stats)
        {
            toReturn += kvp.Key.placeableName + " " + kvp.Value +" || ";
        }
        
        return toReturn;
    }
}
