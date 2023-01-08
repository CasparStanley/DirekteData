using ModelLib;

namespace DirekteDataREST.Managers
{
    public interface IManageDirekteData
    {
        // TODO: Refactor AddData to write to a certain DataSet
        DataStructure GetLive();
        IEnumerable<DataStructure> GetAll();
        DataStructure GetRecordingById(int dataSetId, int id);
        DataSet GetDataSetById(int id);
        DataSet GetDataSetByName(FilterDataSets name);
        DataStructure AddRecording(DataStructure data);
        DataSet AddDataSet(DataSet newDataSet);
        void Update(DataStructure data);
        void DeleteItem(int id);
    }
}
