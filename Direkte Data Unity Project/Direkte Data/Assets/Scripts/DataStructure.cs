using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class corresponds directly to the DataStructure class in the DirekteDataREST project.
/// </summary>
[System.Serializable]
public class DataStructure
{
    public readonly int Id;
    public int DataSetId;
    public int Time;
    public Vector3 Rotation;

    public DataStructure()
    {
    }

    public DataStructure(int id, int dataSetId, int time, Vector3 rotation)
    {
        Id = id;
        DataSetId = dataSetId;
        Time = time;
        Rotation = rotation;
    }
}
