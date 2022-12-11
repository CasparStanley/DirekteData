using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using TMPro;

public class ReadGoogleSheet : MonoBehaviour
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
        StartCoroutine(GetSheetData());
    }

    private IEnumerator GetSheetData()
    {
        UnityWebRequest WWW = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1S5rv8UxE3qOs297lNIvUKe8RNZalGx60yaN46uf8SJ0/values/DD1?key=AIzaSyDhRVF_qWYqCmrbxn4CaphPOimnmzkYD5c");

        yield return WWW.SendWebRequest();

        if (WWW.isNetworkError || WWW.isHttpError)
        {
            Debug.LogError("Web Request Error: " + WWW.error);
        }
        else
        {
            string updateText = "";
            string file = WWW.downloadHandler.text;
            var result = JSON.Parse(file);

            // Digs into feed, and then into entry
            foreach (var item in result["feed"]["entry"])
            {
                var itemObject = JSON.Parse(item.ToString());

                // Building the actual string to display
                updateText += $"{itemObject[0][HEADERS[0]][DT]} | {itemObject[0][HEADERS[1]][DT]} | {itemObject[0][HEADERS[2]][DT]}\n";
            }

            _contentText.text = updateText;
        }
    }
}
