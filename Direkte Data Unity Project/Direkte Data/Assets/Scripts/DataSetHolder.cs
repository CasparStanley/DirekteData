using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSetHolder
{
    public string name;
    public string description;
    public int id;
    public List<DataStructure> Data = new List<DataStructure>();
}
