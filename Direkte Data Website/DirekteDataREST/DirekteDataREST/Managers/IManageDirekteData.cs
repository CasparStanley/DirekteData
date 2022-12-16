using ModelLib;

namespace DirekteDataREST.Managers
{
    public interface IManageDirekteData
    {
        void AddData(DataStructure data);
        public void DeleteItem(int id);
        IEnumerable<DataStructure> GetAll();
        DataStructure GetById(int id);
        void ReplaceList();
        void Update(DataStructure data);

        void GenerateFakeSensorData();
    }
}
