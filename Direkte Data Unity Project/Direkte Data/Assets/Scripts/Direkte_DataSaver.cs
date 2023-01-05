﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// Place the script in the Direkte Data group in the component menu
[AddComponentMenu("DirekteData/Local Unity Data Saver")]
public class Direkte_DataSaver : MonoBehaviour
{
    private static char[] charSep = { ',' };

    public DataSet CurrentDataSet;

    [SerializeField] private DirekteData_DataSO _direkteDataSO;
    private DataSet _direkteDataMockJSON= new DataSet($"Mock JSON Data Set", "", new List<DataStructure>());
    private DataSet _direkteDataReal = new DataSet($"Real Data Set", "", new List<DataStructure>());

    /// <summary>
    /// Save to a new dataset, so we can have multiple datasets to select from
    /// </summary>
    /// <param name="time"></param>
    /// <param name="speed"></param>
    /// <param name="rotation"></param>
    public void SaveToNewLocalDataSet(DataLevel type, int time, int speed, string rotation)
    {
        switch (type)
        {
            case DataLevel.ScriptableObject:
                {
                    CurrentDataSet = _direkteDataSO.FakeRecordings;
                    break;
                }
            case DataLevel.MockJSON:
                {
                    CurrentDataSet = _direkteDataMockJSON;
                    break;
                }
            case DataLevel.Real:
                {
                    CurrentDataSet = _direkteDataReal;
                    break;
                }
            default:
                {
                    CurrentDataSet = new DataSet($"Failed Data Set", "There was an error in saving the dataset, so this was created instead", new List<DataStructure>());
                    break;
                }
        }

        SaveDataSet(time, speed, rotation);
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
        SaveDirekteData(CurrentDataSet, time, speed, rotation);
    }

    /// <summary>
    /// Switch and then save the current dataset
    /// </summary>
    /// <param name="time"></param>
    /// <param name="speed"></param>
    /// <param name="rotation"></param>
    public void SaveDataSet(DataLevel type, int time, int speed, string rotation)
    {
        SwitchCurrentDataSet(type);

        SaveDirekteData(CurrentDataSet, time, speed, rotation);
    }

    /// <summary>
    /// Get the current dataset
    /// </summary>
    /// <returns>The current dataset</returns>
    public DataSet GetDataSet(DataLevel type)
    {
        SwitchCurrentDataSet(type);

        return CurrentDataSet;
    }

    private bool SaveDirekteData(DataSet data, int time, int speed, string rotation)
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

        // Add the data to the selected dataset
        data.Recordings.Add(new DataStructure(time, speed, newRot));

        return true;
    }

    private bool SwitchCurrentDataSet(DataLevel type)
    {
        switch (type)
        {
            case DataLevel.ScriptableObject:
                {
                    CurrentDataSet = _direkteDataSO.FakeRecordings;
                    break;
                }
            case DataLevel.MockJSON:
                {
                    CurrentDataSet = _direkteDataMockJSON;
                    break;
                }
            case DataLevel.Real:
                {
                    CurrentDataSet = _direkteDataReal;
                    break;
                }
            default:
                {
                    Debug.LogError("There was an error in the switching of the current data set type");
                    CurrentDataSet = _direkteDataSO.FakeRecordings;
                    return false;
                }
        }

        if (CurrentDataSet == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}