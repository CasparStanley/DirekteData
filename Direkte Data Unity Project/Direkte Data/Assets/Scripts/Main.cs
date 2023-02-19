using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public DirekteData_ObjectMover ObjectToHandle;

    // 20 miliseconds is 0,02 seconds
    // we should receive data once every 0,066 seconds so let's check more often to be safe
    public float UpdateTime = 0.3f;

    [SerializeField] private Direkte_DataSaver dataHandler;

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

            yield return new WaitForSeconds(UpdateTime);
        }
    }
}
