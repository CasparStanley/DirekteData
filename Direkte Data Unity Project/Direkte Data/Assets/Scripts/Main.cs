using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [Header("References")]
    public DirekteData_ObjectMover ObjectToHandle;
    [SerializeField] private Direkte_DataSaver dataHandler;

    [Header("Settings")]
    // 30 miliseconds is 0,03 seconds
    // we should receive data once every 0,033 seconds so let's check more often to be safe
    public float UpdateTime = 0.03f;


    private void Start()
    {
        StartCoroutine(MoveObject());
        //ObjectToHandle.MoveFromDatabase();
    }

    public IEnumerator MoveObject()
    {
        while (true)
        {
            DataStructure recording = dataHandler.LatestRecording;

            //Debug.Log($"Moving object: x={recording.Rotation.x}, y={recording.Rotation.y}, z={recording.Rotation.z}");

            ObjectToHandle.MoveLive(recording);

            // Update at the specified "frame rate" and then multiply that by the "resolution"
            // 1 - resolution so in Unity "10" is the best resolution and "1" is very slow updates
            yield return new WaitForSeconds(UpdateTime);
        }
    }
}
