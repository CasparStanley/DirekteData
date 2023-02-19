using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageDirekteData : IManageDirekteData
{
    // Lazy Singleton
    private static readonly Lazy<ManageDirekteData> _instance = new Lazy<ManageDirekteData>(() => new ManageDirekteData());
    public static ManageDirekteData Instance { get { return _instance.Value; } }

    // TODO: Instead of this mock recordings use the current data set in Direkte_DataSaver
    private static List<DataStructure> _mockRecordings = new List<DataStructure>
    {
        new DataStructure(0, "0,0,0"),
        new DataStructure(1, "0,1,0"),
        new DataStructure(2, "0,5,0"),
        new DataStructure(3, "0,10,0"),
        new DataStructure(4, "0,15,0")
    };

    public ManageDirekteData() { }

    public DataStructure AddRecording(DataStructure data)
    {
        _mockRecordings.Add(data);
        return data;
    }

    public void DeleteItem(int id)
    {
        if (_mockRecordings[id] != null)
        {
            _mockRecordings.RemoveAt(id);
        }
    }

    public IEnumerable<DataStructure> GetAll()
    {
        return _mockRecordings;
    }

    public DataSet GetDataSetById(int id)
    {
        throw new NotImplementedException();
    }

    public DataStructure GetRecordingById(int dataSetId, int id)
    {
        return _mockRecordings[id];
    }

    public void Update(DataStructure data)
    {
        throw new NotImplementedException();
    }

    public IEnumerator GenerateFakeSensorData()
    {
        int updateFrequency = 1;
        int fakeTime = 0;

        string fakeData = "0,0,0";

        bool running = true;

        while (running)
        {
            fakeTime += updateFrequency;

            int fakeX = new System.Random().Next(0, 359);
            int fakeY = new System.Random().Next(0, 359);
            int fakeZ = new System.Random().Next(0, 359);

            string fakeRotation = $"{fakeX},{fakeY},{fakeZ}";

            fakeData = $"{fakeTime},{fakeRotation}";

            Console.WriteLine(fakeData);

            DataStructure dataObj = new DataStructure(fakeTime, fakeRotation);

            AddRecording(dataObj);

            yield return new WaitForSeconds(updateFrequency);
        }
    }

    public override string ToString()
    {
        return $"Manage direkte data";
    }

    public DataSet AddDataSet(DataSet newDataSet)
    {
        throw new NotImplementedException();
    }

    public DataSet GetDataSetByName(FilterDataSets name)
    {
        throw new NotImplementedException();
    }

    public DataStructure GetLive()
    {
        throw new NotImplementedException();
    }
}
