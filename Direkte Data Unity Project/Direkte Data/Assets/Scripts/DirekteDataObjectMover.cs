using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirekteDataObjectMover : MonoBehaviour
{
    [SerializeField] private DirekteDataSaver dataSaver;
    [SerializeField] private DataLevel loadDataType;

    private List<int> timeDelays = new List<int>();
    private List<int> speeds = new List<int>();
    private List<Vector3> rotations = new List<Vector3>();

    public void StartMoving()
    {
        foreach (var data in dataSaver.GetDataSet(loadDataType).Data)
        {
            timeDelays.Add(data.Time);
            speeds.Add(data.Speed);
            rotations.Add(data.Rotation);
        }

        StartCoroutine(ControlMovement());
    }

    private IEnumerator ControlMovement()
    {
        for (int i = 0; i < dataSaver.GetDataSet(loadDataType).Data.Count; i++)
        {
            // Wait for the difference between the current "time" and the previous "time"
            if (i == 0)
            {
                yield return new WaitForSeconds(timeDelays[i]);
            }
            else
            {
                yield return new WaitForSeconds(timeDelays[i] - timeDelays[i - 1]);
            }

            gameObject.transform.Rotate(rotations[i]);
        }
    }
}
