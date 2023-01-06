using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class corresponds directly to the DataStructure class in the DirekteDataREST project.
/// </summary>
[System.Serializable]
public class DataStructure
{
    public int Time;
    public Vector3 Rotation;

    public DataStructure()
    {
    }

    public DataStructure(int time, Vector3 rotation)
    {
        Time = time;
        Rotation = rotation;
    }
}
