using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

// Place the script in the Direkte Data group in the component menu
[AddComponentMenu("DirekteData/Local Unity Data Saver")]
public class Direkte_DataSaver : MonoBehaviour
{
    private static char[] charSep = { ',' };

    [SerializeField] private Main _main;
    [SerializeField] private DirekteData_DataSO _direkteDataSO;

    public DataSet CurrentDataSet;

    private DataSet _direkteDataMockJSON = new DataSet(0, $"Mock JSON Data Set", "", new List<DataStructure>());
    private DataSet _direkteDataReal = new DataSet(0, $"Direkte Data Set", "", new List<DataStructure>());

    public DataStructure LatestRecording;

    // TODO: Make a _direkteData_Database which will be a loaded dataset from the database that you choose

    /// <summary>
    /// Save to a new dataset, so we can have multiple types of datasets to select from
    /// We can have a ScriptableObject type, which is a local dataset you can set up in Unity.
    /// We can have a MockJSON which uses the mock data from the REST service
    /// Finally, we can use the real data set from the sensor/database
    /// </summary>
    /// <param name="time"></param>
    /// <param name="rotation"></param>
    public void ChooseDataSet(DataLevel type)
    {
        SwitchCurrentDataSet(type);
    }

    /// <summary>
    /// Save to the current dataset
    /// </summary>
    /// <param name="time"></param>
    /// <param name="rotation"></param>
    public void SaveRecording(float time, string rotation, int id = 0, int dataSetId = 0)
    {
        // Save to the current dataset - by calling GetDataSet() we make sure that if it doesn't exist, a new one is created and set as the current Dataset
        AddRecordingToDataSet(ParseRecordingToDataSet(time, rotation, id, dataSetId), CurrentDataSet);
    }

    /// <summary>
    /// Save to the current dataset
    /// </summary>
    /// <param name="time"></param>
    /// <param name="rotation"></param>
    public void SaveDataSet(DataSet dataSet)
    {
        // Save to the current dataset - by calling GetDataSet() we make sure that if it doesn't exist, a new one is created and set as the current Dataset
        CurrentDataSet.Name = dataSet.Name;
        CurrentDataSet.Description = dataSet.Description;
    }

    /// <summary>
    /// Switch and then save the current dataset
    /// </summary>
    /// <param name="time"></param>
    /// <param name="rotation"></param>
    public void SaveRecording(DataLevel type, float time, string rotation, int id = 0, int dataSetId = 0)
    {
        Debug.Log($"DataSaver saving recording: Time={time}, Rotation={rotation}");
        SwitchCurrentDataSet(type);

        AddRecordingToDataSet(ParseRecordingToDataSet(time, rotation, id, dataSetId), CurrentDataSet);
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

    public DataStructure ParseRecordingToDataSet(float time, string rotation, int id = 0, int dataSetId = 0)
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
            return null;
        }

        // First we need to convert the rotation string to a Vector 3
        Vector3 newRot = new Vector3(rotX, rotY, rotZ);

        // Add the data to the selected dataset
        DataStructure recording = new DataStructure(id, dataSetId, time, newRot);

        Debug.Log("New recording added! " + recording.ToString());

        LatestRecording = recording;
        return recording;
    }

    private void AddRecordingToDataSet(DataStructure recording, DataSet dataSet)
    {
        dataSet.Recordings.Add(recording);
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
