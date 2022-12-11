using ModelLib;

namespace DirekteDataREST.Managers
{
    public interface IManageDirekteData
    {
        IEnumerable<DataStructure> GetAll();
        void Update(DataStructure data);
        DataStructure GetById(int id);
        void AddSubject(DataStructure data);
        void ReplaceList();
        public void DeleteItem(int id);
    }
}
