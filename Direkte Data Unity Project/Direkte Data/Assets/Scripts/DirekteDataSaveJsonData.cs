using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirekteDataSaveJsonData : MonoBehaviour
{
    private static char[] charSep = { ',' };

    public bool SaveDirekteData(DirekteData_DataSO dataset, int time, int speed, string rotation)
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
            return false;
        }

        // First we need to convert the rotation string to a Vector 3
        Vector3 newRot = new Vector3(rotX, rotY, rotZ);

        // Then we can save the new data!
        dataset.Dataset.Add(new DataStructure(time, speed, newRot));

        return true;
    }
}
