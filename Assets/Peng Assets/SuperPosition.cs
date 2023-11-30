using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPosition
{
    List<CellStats> _possibleValues = new List<CellStats>();
    bool _observed = false;

    public SuperPosition(List<CellStats> allProtoData)
    {
        foreach (var VARIABLE in allProtoData)
        {
            _possibleValues.Add(VARIABLE);
        }
    }

    public CellStats GetObservedValue()
    {
        return _possibleValues[0];
    }

    public CellStats Observe()
    {
        //pick one of the possible values at random and then remove all other possible values
        //then set _observed to true
        //return the observed value

        CellStats chosenValue = _possibleValues[Random.Range(0,_possibleValues.Count)];
        _possibleValues = new List<CellStats> { chosenValue};
        _observed= true;

        return GetObservedValue();
    }
    
    public void Refill(List<CellStats> allProtoData)
    {
        _possibleValues.Clear();
        foreach (var VARIABLE in allProtoData)
        {
            _possibleValues.Add(VARIABLE);
        }
        _observed = false;
    }

    public CellStats AssignObservation(CellStats ppd)
    {
        _possibleValues = new List<CellStats> { ppd };
        _observed = true;
        return ppd;
    }

    public bool IsObserved()
    {
        return _observed;
    }

    public void RemovePossibleValue(CellStats value)
    {
        _possibleValues.Remove(value);
    }

    public bool HasPossibilities()
    {
        return _possibleValues.Count > 0;
    }

    public List<CellStats> RemainPossibleValues()
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
