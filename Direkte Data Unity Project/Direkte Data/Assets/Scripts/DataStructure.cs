using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class corresponds directly to the DataStructure class in the DirekteDataREST project.
/// </summary>
[System.Serializable]
public class DataStructure
{
    public int Id;
    public int DataSetId;
    public float Time;
    public Vector3 Rotation;
    public string RotationNotConverted;

    public DataStructure()
    {
    }

    public DataStructure(float time, string rotation)
    {
        Time = time;
        RotationNotConverted = rotation;
    }

    public DataStructure(int id, int dataSetId, float time, string rotation)
    {
        Id = id;
        DataSetId = dataSetId;
        Time = time;
        RotationNotConverted = rotation;
    }

    public DataStructure(int id, int dataSetId, float time, Vector3 rotation)
    {
        Id = id;
        DataSetId = dataSetId;
        Time = time;
        Rotation = rotation;
    }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id.ToString()}, {nameof(DataSetId)}: {DataSetId.ToString()}, {nameof(Time)} = {Time.ToString()}, {nameof(Rotation)} = {Rotation}";
    }
}
