using ModelLib;

namespace DirekteDataREST.Managers
{
    public interface IManageDirekteData
    {
        // TODO: Refactor AddData to write to a certain DataSet
        DataStructure AddData(DataStructure data);
        void DeleteItem(int id);
        IEnumerable<DataStructure> GetAll();
        DataStructure GetById(int id);
        void Update(DataStructure data);

        void GenerateFakeSensorData();
    }
}
