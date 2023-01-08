using ModelLib;

namespace DirekteDataREST.Managers
{
    public interface IManageDirekteData
    {
        // TODO: Refactor AddData to write to a certain DataSet
        DataStructure AddData(DataStructure data);
        void DeleteItem(int id);
        IEnumerable<DataStructure> GetAll();
        DataStructure GetRecordingById(int dataSetId, int id);
        DataSet GetDataSetById(int id);
        void Update(DataStructure data);

        void GenerateFakeSensorData();
    }
}
