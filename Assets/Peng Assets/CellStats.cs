public class CellStats
{
    public int amount;

    public CellStats()
    {
        amount = 0;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is CellStats)) return false;
        return amount == ((CellStats)obj).amount;
    }
}
