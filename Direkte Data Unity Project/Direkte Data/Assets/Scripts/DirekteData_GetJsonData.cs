using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using TMPro;
using System.Data;
using UnityEngine.Events;

// Place the script in the Direkte Data group in the component menu
[AddComponentMenu("DirekteData/JSON Loader")]
public class DirekteData_GetJsonData : MonoBehaviour
{
    private static string[] HEADERS = { "id", "dataSetId", "time", "rotation" };

    [SerializeField] private Direkte_DataSaver _dataSaver;
    [SerializeField] private TMP_Text _contentText;
    [SerializeField] private bool _updateRealtime;

    [Space(10)]

    public bool LoadFromDatabase = false;

    [Space(10)]

    public UnityEvent OnDataLoaded;

    //private void Start()
    //{
    //    UpdateData();
    //}

    private void Update()
    {
        if (LoadFromDatabase)
        {
            // REALLY QUITE BAD ACTUALLY, maybe just update each time a new data recording is expected?
            if (_updateRealtime)
            {
                UpdateData();
            }
        }
    }

    public void UpdateData()
    {
        StartCoroutine(GetData());
    }

    private IEnumerator GetData()
    {
        // TODO: Add functionality to get specific dataset ("/api/DirekteData/1" for eksempel)

        // Request the JSON file from the website
        UnityWebRequest WWW = UnityWebRequest.Get("https://direktedatarest2022.azurewebsites.net/api/DirekteData/live");

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
            var result = JSON.Parse(file);

            // Digs into each object
            foreach (var item in result)
            {
                // Parse each column to a new 'var'-object
                var itemObject = JSON.Parse(item.ToString());

                // Building the actual string to display for debugging purposes
                updateText += $"{HEADERS[0]}: {itemObject[0][HEADERS[0]]} | " +
                              $"{HEADERS[1]}: {itemObject[0][HEADERS[1]]} | " +
                              $"{HEADERS[2]}: {itemObject[0][HEADERS[2]]} | " +
                              $"{HEADERS[3]}: {itemObject[0][HEADERS[3]]}\n";

                // Save to the current dataset
                _dataSaver.SaveRecording(DataLevel.Real, 
                                        itemObject[0][HEADERS[0]], 
                                        itemObject[0][HEADERS[1]], 
                                        itemObject[0][HEADERS[2]], 
                                        itemObject[0][HEADERS[3]]);
            }

            // Update the text shown on screen
            _contentText.text = updateText;

            // Trigger any methods that are waiting for the data to be loaded
            OnDataLoaded?.Invoke();
        }
    }
}
