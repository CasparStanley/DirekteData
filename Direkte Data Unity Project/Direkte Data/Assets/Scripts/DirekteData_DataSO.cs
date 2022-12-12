using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirekteData_DataSO : ScriptableObject
{
    // This is a scriptable object that contains a list of objects, that correspond to the C# class "DataStructure" in DirekteDataREST
    public List<DataStructure> Dataset = new List<DataStructure>();
}
