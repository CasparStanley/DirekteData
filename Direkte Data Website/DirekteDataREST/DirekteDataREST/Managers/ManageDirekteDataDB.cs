using ModelLib;
using System.Xml.Linq;

namespace DirekteDataREST.Managers
{
    public class ManageDirekteDataDB : IManageDirekteData
    {
        // Laver manageren til en singleton så den kan refereres i andre dele af programmet, specifikt Sensor Receiver
        
        private readonly DirekteDataContext _context;

        public ManageDirekteDataDB(DirekteDataContext context)
        {
            _context = context;
        }

        public DataStructure AddData(DataStructure data)
        {
            _context.Recordings.Add(data);
            _context.SaveChanges();
            return data;
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
            return _context.Recordings.ToList();
        }

        public DataStructure GetById(int id)
        {
            if (_context.Recordings.ToList().Exists(i => i.Id == id))
            {
                DataStructure? recordings = _context.Recordings.Find(id);
                return recordings;
            }
            throw new KeyNotFoundException();
        }

        public void Update(DataStructure data)
        {
            DataStructure recording = GetById(data.Id);
            //_context.Entry(recording).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            _context.Recordings.Update(recording);
            _context.SaveChanges();
        }

        public override string ToString()
        {
            return $"Manage direkte data - Database";
        }
    }
}
