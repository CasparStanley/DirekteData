using ModelLib;

namespace DirekteDataREST.Managers
{
    public class ManageDirekteDataDB : IManageDirekteData
    {
        private readonly DirekteDataContext _context;

        public ManageDirekteDataDB(DirekteDataContext context)
        {
            _context = context;
        }

        public void AddData(DataStructure data)
        {
            _context.Recordings.Add(data);
            _context.SaveChanges();
            //return true;
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public void GenerateFakeSensorData()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataStructure> GetAll()
        {
            return _context.Recordings;
        }

        public DataStructure GetById(int id)
        {
            return _context.Recordings.Find(id);
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
