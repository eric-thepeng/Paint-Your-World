using System.Collections.Generic;

public class CellStats
{
    public int amount;
    public static List<CellStats> allPossibilities = new List<CellStats>();

    public CellStats()
    {
        amount = 0;
    }
    

    public override bool Equals(object obj)
    {
        if (!(obj is CellStats)) return false;
        return amount == ((CellStats)obj).amount;
    }

    public override string ToString()
    {
        return "Cell Stats is: " + amount;
    }
}
