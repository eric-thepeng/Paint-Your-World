using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuperPosition
{
    public class Proto
    {
        public CellStats cellStats;
        public int weight;

        public Proto(CellStats cellStats, int amount)
        {
            this.cellStats = cellStats;
            this.weight = amount;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Proto)) return false;
            Proto otherProto = (Proto)obj;

            if (otherProto.cellStats == this.cellStats && otherProto.weight == this.weight) return true;
            
            return false;
        }
    }
    
    List<Proto> _possibleValues = new List<Proto>();
    bool _observed = false;

    /*
    public SuperPosition(List<CellStats> allProtoData)
    {
        foreach (var VARIABLE in allProtoData)
        {
            _possibleValues.Add(VARIABLE);
        }
    }*/

    public SuperPosition(List<Proto> weightedProtoData)
    {
        foreach (var proto in weightedProtoData)
        {
            _possibleValues.Add(proto);
        }
    }

    public CellStats GetObservedValue()
    {
        return _possibleValues[0].cellStats;
    }

    public CellStats Observe()
    {

        /*
        List<CellStats> keys = _possibleValues.Keys.ToList();
        List<int> values = _possibleValues.Values.ToList();

        int sum = 0;
        foreach (var VARIABLE in values)
        {
            sum += VARIABLE;
        }

        int pick = Random.Range(0, sum);
        for (int i = 0; i < values.Count; i++)
        {
            pick -= values[i];
            if (pick <= 0)
            {       
                _observed= true;
                _possibleValues = new Dictionary<CellStats, int>() { { keys[i], values[i] } };
                return GetObservedValue();
            }
        }
        */

        int sum = 0;
        foreach (var VARIABLE in _possibleValues)
        {
            sum += VARIABLE.weight;
        }
        int pick = Random.Range(0, sum);
        foreach (var VARIABLE in _possibleValues)
        {
            pick -= VARIABLE.weight;
            if (pick <= 0)
            {
                _observed = true;
                _possibleValues = new List<Proto>() {VARIABLE };
                return GetObservedValue();
            }
        }

        return GetObservedValue();
    }
    
    /*
    public void Refill(Dictionary<CellStats,int> weightedProtoData)
    {
        _possibleValues.Clear();
        foreach (var pair in weightedProtoData)
        {
            _possibleValues.Add(pair.Key,pair.Value);
        }
        _observed = false;
    }*/

    public void AssignObservation(CellStats cellStats)
    {
        _possibleValues = new List<Proto>(){new Proto(cellStats,1)} ;
        _observed = true;
    }

    public bool IsObserved()
    {
        return _observed;
    }

    /*
    public void RemovePossibleValue(CellStats value)
    {
        _possibleValues.Remove(value);
    }*/

    public bool HasPossibilities()
    {
        return _possibleValues.Count > 0;
    }

    public List<Proto> RemainPossibleValues()
    {
        return _possibleValues;
    }

    public int NumOptions{
        get
        {
            return _possibleValues.Count;
        }
    }

}
