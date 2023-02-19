using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using TMPro;
using System.Data;
using UnityEngine.Events;
using System.Text.RegularExpressions;

// Place the script in the Direkte Data group in the component menu
[AddComponentMenu("DirekteData/JSON Loader")]
public class DirekteData_GetJsonData : MonoBehaviour
{
    private static string[] DATASET_HEADERS = { "id", "name", "description", "recordings" };
    private static string[] RECORDING_HEADERS = { "id", "dataSetId", "time", "rotation" };

    [SerializeField] private Direkte_DataSaver _dataSaver;
    [SerializeField] private TMP_Text _contentText;
    [SerializeField] private bool _updateRealtime;

    [Space(10)]

    public bool LoadFromDatabase = false;

    [Space(10)]

    public UnityEvent OnDataLoaded;

    private List<DataStructure> recordingsToParse = new List<DataStructure>();

    private void Start()
    {
        //UpdateData();
    }

    private void Update()
    {
        // REALLY QUITE BAD ACTUALLY, maybe just update each time a new data recording is expected?
        //if (_updateRealtime)
        //{
        //    UpdateData();
        //}
    }

    public void UpdateData()
    {
        StartCoroutine(GetData());
    }

    private IEnumerator GetData()
    {
        // TODO: Add functionality to get specific dataset ("/api/DirekteData/1" for eksempel)

        // Choose to save to the real dataset
        _dataSaver.ChooseDataSet(DataLevel.Real);

        // Request the JSON file from the website
        // TODO: Not this - just get live from UDP receiver.
        UnityWebRequest WWW = UnityWebRequest.Get("https://direktedatarest2022.azurewebsites.net/api/DirekteData/1");

        // Wait for the request to return
        yield return WWW.SendWebRequest();

        // Unless there's an error we are good to proceed!
        if (WWW.isNetworkError || WWW.isHttpError)
        {
            Debug.LogError("Web Request Error: " + WWW.error);
        }
        else
        {
            string updateText = "";

            // Converts the raw bytes to a string
            string file = WWW.downloadHandler.text;
            // Parse the downloaded file to a 'var'-object
            JSONNode dataSetJson = JSON.Parse(file);

            Debug.Log($"JSON: {dataSetJson}");

            _dataSaver.SaveDataSet(ManualParseDataSet(dataSetJson));

            // Update the text shown on screen
            _contentText.text = updateText;

            // Trigger any methods that are waiting for the data to be loaded
            OnDataLoaded?.Invoke();
        }

        //_dataSaver.SaveDataSet(i[0][DATASET_HEADERS[1]],  // Name
        //                       i[0][DATASET_HEADERS[2]]); // Description

        //_dataSaver.SaveRecording(r[0][RECORDING_HEADERS[2]],  // Time
        //                         r[0][RECORDING_HEADERS[3]]); // Rotation

        //// Building the actual string to display for debugging purposes
        //updateText += $"{DATASET_HEADERS[0]}: {ds[0][DATASET_HEADERS[0]]} | " + // Id
        //              $"{DATASET_HEADERS[1]}: {ds[0][DATASET_HEADERS[1]]} | " + // Name
        //              $"{DATASET_HEADERS[2]}: {ds[0][DATASET_HEADERS[2]]}\n\n"; // Description

        //// Building the actual string to display for debugging purposes
        //updateText += $"{RECORDING_HEADERS[2]}: {ds[0][RECORDING_HEADERS[2]]} | " + // Time
        //              $"{RECORDING_HEADERS[3]}: {ds[0][RECORDING_HEADERS[3]]}\n"; // Rotation
    }

    private DataSet ManualParseDataSet(JSONNode dataSetJson)
    {
        // Digs into each object
        DataSet dataSet = new DataSet();
        

        foreach (var item in dataSetJson)
        {
            var i = JSON.Parse(item.ToString());
            Debug.Log($"dataSet item: {i}");
            Debug.Log($"item name: {i[0]}");
            Debug.Log($"item value: {i[1]}");

            switch (i[0].ToString())
            {
                case "\"name\"":
                    dataSet.Name = i[1];
                    break;
                case "\"description\"":
                    dataSet.Description = i[1];
                    break;
                case "\"recordings\"":
                    ManualParseRecording(i[1]);
                    break;
            }
        }

        return dataSet;
    }

    private void ManualParseRecording(JSONNode recordingJson)
    {
        Debug.Log($"RECORDING JSON: {recordingJson}");

        // Digs into each object
        DataStructure recording = new DataStructure();
        foreach (var item in recordingJson)
        {
            var i = JSON.Parse(item.ToString());
            var r = JSON.Parse(i[0].ToString());

            Debug.Log($"recording item: {r}");
            Debug.Log($"item id: {r[0]}");
            Debug.Log($"item dataset id: {r[1]}");
            Debug.Log($"item time: {r[2]}");
            Debug.Log($"item rotation: {r[3]}");

            //recording.Id= r[0];
            //recording.DataSetId = r[1];
            //recording.Time = r[2];
            //recording.RotationNotConverted = r[3];

            _dataSaver.SaveRecording(r[2], r[3], r[0], r[1]);
        }
    }
}
