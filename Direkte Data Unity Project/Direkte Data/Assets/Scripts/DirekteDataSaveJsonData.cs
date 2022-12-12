using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DirekteDataSaveJsonData : MonoBehaviour
{
    private static char[] charSep = { ',' };

    [SerializeField] private DirekteData_DataSO _currentDataSet;

    /// <summary>
    /// Save to a new dataset, so we can have multiple datasets to select from
    /// </summary>
    /// <param name="time"></param>
    /// <param name="speed"></param>
    /// <param name="rotation"></param>
    public void SaveToNewDataSet(int time, int speed, string rotation)
    {
        _currentDataSet = ScriptableObject.CreateInstance<DirekteData_DataSO>();
        SaveDirekteData(_currentDataSet, time, speed, rotation);
    }

    /// <summary>
    /// Save to the current dataset
    /// </summary>
    /// <param name="time"></param>
    /// <param name="speed"></param>
    /// <param name="rotation"></param>
    public void SaveDataSet(int time, int speed, string rotation)
    {
        // Save to the current dataset - by calling GetDataSet() we make sure that if it doesn't exist, a new one is created and set as the current Dataset
        SaveDirekteData(GetDataSet(), time, speed, rotation);
    }

    /// <summary>
    /// Get the current dataset
    /// </summary>
    /// <returns>The current dataset</returns>
    public DirekteData_DataSO GetDataSet()
    {
        if ( _currentDataSet == null)
        {
            _currentDataSet = ScriptableObject.CreateInstance<DirekteData_DataSO>();
        }
        
        return _currentDataSet;
    }

    private bool SaveDirekteData(DirekteData_DataSO dataSO, int time, int speed, string rotation)
    {
        float rotX, rotY, rotZ;
        try
        {
            rotX = float.Parse(rotation.Split(charSep)[0]);
            rotY = float.Parse(rotation.Split(charSep)[1]);
            rotZ = float.Parse(rotation.Split(charSep)[2]);
        }
        catch
        {
            Debug.LogError($"The rotation formatting was wrong! Expected: '0,0,0' - Received: {rotation}");
            return false;
        }

        // First we need to convert the rotation string to a Vector 3
        Vector3 newRot = new Vector3(rotX, rotY, rotZ);

        // Then we can add the new data to the scriptable object dataset
        //dataSO.DataSets.Data.Add(new DataStructure(time, speed, newRot));

        return true;
    }
}
