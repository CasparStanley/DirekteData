using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSetHolder
{
    public string name;
    public string description;
    public readonly int id;
    public List<DataStructure> Data = new List<DataStructure>();

    private static int _currentId = 0;

    public DataSetHolder(string name, string description, List<DataStructure> data)
    {
        this.name = name;
        this.description = description;
        this.id = _currentId;
        Data = data;

        _currentId++;
    }
}
