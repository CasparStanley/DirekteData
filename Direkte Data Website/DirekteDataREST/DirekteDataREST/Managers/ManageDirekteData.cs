using ModelLib;

namespace DirekteDataREST.Managers
{
    public class ManageDirekteData : IManageDirekteData
    {
        private static List<DataStructure> _mockRecordings = new()
        {
            new DataStructure(0, 0, "0,0,0"),
            new DataStructure(1, 1, "0,1,0"),
            new DataStructure(2, 2, "0,5,0"),
            new DataStructure(3, 4, "0,10,0"),
            new DataStructure(4, 10, "0,15,0")
        };
        public void AddSubject(DataStructure data)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataStructure> GetAll()
        {
            return _mockRecordings;
        }

        public DataStructure GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void ReplaceList()
        {
            throw new NotImplementedException();
        }

        public void Update(DataStructure data)
        {
            throw new NotImplementedException();
        }
    }
}
