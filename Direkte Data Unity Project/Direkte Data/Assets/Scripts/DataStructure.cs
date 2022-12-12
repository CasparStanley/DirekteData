using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class corresponds directly to the DataStructure class in the DirekteDataREST project.
/// </summary>
public class DataStructure
{
    public int Time;
    public int Speed;
    public Vector3 Rotation;

    public DataStructure()
    {
    }

    public DataStructure(int time, int speed, Vector3 rotation)
    {
        Time = time;
        Speed = speed;
        Rotation = rotation;
    }
}
