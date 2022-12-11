using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using TMPro;

public class ConvertJsonString : MonoBehaviour
{
    private static string[] HEADERS = { "gsx$time", "gsx$speed", "gsx$rotation" };
    private static string DT = "$t";

    [SerializeField] private TMP_Text _contentText;

    [SerializeField] private bool _updateRealtime;

    private void Start()
    {
        UpdateData();
    }

    private void Update()
    {
        // REALLY QUITE BAD ACTUALLY
        if (_updateRealtime)
        {
            UpdateData();
        }
    }

    public void UpdateData()
    {
        StartCoroutine(GetData());
    }

    private IEnumerator GetData()
    {
        // Request the JSON file from the website
        UnityWebRequest WWW = UnityWebRequest.Get("");

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
            // Parse the downloaded file to an object
            var result = JSON.Parse(file);

            // Digs into feed, and then into entry to get the list of items - the columns
            foreach (var item in result["feed"]["entry"])
            {
                // Parse each column to an object
                var itemObject = JSON.Parse(item.ToString());

                // Building the actual string to display
                updateText += $"{itemObject[0][HEADERS[0]][DT]} | {itemObject[0][HEADERS[1]][DT]} | {itemObject[0][HEADERS[2]][DT]}\n";
            }

            // Update the text shown on screen
            _contentText.text = updateText;
        }
    }
}
