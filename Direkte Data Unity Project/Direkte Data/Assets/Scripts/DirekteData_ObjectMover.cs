using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Place the script in the Direkte Data group in the component menu
[AddComponentMenu("DirekteData/Object Mover")]
public class DirekteData_ObjectMover : MonoBehaviour
{
    [HideInInspector] public bool animationDone = true;

    [SerializeField] private Direkte_DataSaver dataHandler;
    [SerializeField] private DataLevel loadDataType;

    [SerializeField] private AnimationCurve rotationCurve;

    private List<float> timeDelays = new List<float>();
    private List<Vector3> rotations = new List<Vector3>();

    private float rotationTime = 0.066f;

    public void MoveFromDatabase()
    {
        StartCoroutine(WaitABitThenStart());
    }

    public void MoveLive(DataStructure recording)
    {
        RotateSmooth(gameObject.transform, recording.Rotation, rotationTime, rotationCurve);
    }

    private IEnumerator ControlMovement()
    {
        Debug.Log("Controlling movement... recordings: " + dataHandler.GetDataSet(loadDataType).Recordings.Count);

        for (int i = 0; i < dataHandler.GetDataSet(loadDataType).Recordings.Count; i++)
        {
            Debug.Log(i + "/" + dataHandler.GetDataSet(loadDataType).Recordings.Count);

            // Wait for the difference between the current "time" and the previous "time"
            if (i == 0)
            {
                yield return new WaitForSeconds(timeDelays[i]);
            }
            else
            {
                yield return new WaitForSeconds(timeDelays[i] - timeDelays[i - 1] * 10);
            }

            gameObject.transform.Rotate(rotations[i]);
        }
    }

    public IEnumerator RotateSmooth(Transform objTransform, Vector3 targetRot, float duration, AnimationCurve curve, Vector3? fromRot = null)
    {
        animationDone = false;

        Vector3 startRot = fromRot ?? objTransform.localEulerAngles;
        Vector3 endRot = targetRot;

        for (float t = 0f; t < 1f; t += Time.deltaTime / duration)
        {
            if (objTransform != null)
            {
                float s = curve.Evaluate(t);
                objTransform.localEulerAngles = Vector3.Lerp(startRot, endRot, s);
                yield return null;
            }
        }

        if (objTransform != null)
            objTransform.localEulerAngles = targetRot;

        animationDone = true;
    }

    private IEnumerator WaitABitThenStart()
    {
        yield return new WaitForSeconds(5);

        foreach (var data in dataHandler.GetDataSet(loadDataType).Recordings)
        {
            timeDelays.Add(data.Time);
            rotations.Add(data.Rotation);
        }

        StartCoroutine(ControlMovement());
    }
}
