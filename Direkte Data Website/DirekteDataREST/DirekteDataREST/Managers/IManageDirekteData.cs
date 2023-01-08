﻿using ModelLib;

namespace DirekteDataREST.Managers
{
    public interface IManageDirekteData
    {
        // TODO: Refactor AddData to write to a certain DataSet
        IEnumerable<DataStructure> GetAll();
        DataStructure GetRecordingById(int dataSetId, int id);
        DataSet GetDataSetById(int id);
        DataStructure AddRecording(DataStructure data);
        DataSet AddDataSet(DataSet newDataSet);
        void Update(DataStructure data);
        void DeleteItem(int id);
    }
}
