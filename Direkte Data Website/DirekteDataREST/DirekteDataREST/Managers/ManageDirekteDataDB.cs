using ModelLib;
using System.Diagnostics;
using System.Xml.Linq;
using DirekteDataREST.SensorReceiver;

namespace DirekteDataREST.Managers
{
    public class ManageDirekteDataDB : IManageDirekteData
    {
        private readonly DirekteDataContext _context;

        public ManageDirekteDataDB(DirekteDataContext context)
        {
            _context = context;
        }
        public IEnumerable<DataStructure> GetAll()
        {
            StartSensorReceiver();
            return _context.Recordings.ToList();
        }

        public DataStructure GetRecordingById(int dataSetId, int id)
        {
            // just make sure that any recordings exist within the given dataset
            if (_context.Recordings.ToList().Exists(i => i.DataSetId == dataSetId))
            {
                //  Get all the recordings in this dataset
                List<DataStructure> recordingsInDataSet = _context.Recordings.Where(r => r.DataSetId == dataSetId).ToList();

                // then make sure the recording with the Id exists in the selected dataset
                if (recordingsInDataSet.Exists(i => i.Id == id))
                {
                    DataStructure? recording = recordingsInDataSet.Where(r => r.Id == id).ToList()[0];
                    return recording;
                }
            }
            throw new KeyNotFoundException();
        }

        public DataSet GetDataSetById(int id)
        {
            if (_context.DataSets.ToList().Exists(i => i.Id == id))
            {
                // Find the dataset in the database
                DataSet? dataSet = _context.DataSets.Find(id);
                
                // Add all the recordings that should be in this dataset,
                // since that doesn't come from the same table in the database
                List<DataStructure> recordingsInDataSet = _context.Recordings.Where(r => r.DataSetId == id).ToList();
                foreach (DataStructure recording in recordingsInDataSet)
                {
                    // Just double check that we didn't already add the recording
                    if (!dataSet.Recordings.Contains(recording))
                    {
                        dataSet.Recordings.Add(recording);
                    }
                }

                return dataSet;
            }
            throw new KeyNotFoundException();
        }

        public DataSet GetDataSetByName(FilterDataSets filter)
        {
            if (_context.DataSets.ToList().Exists(i => i.Name == filter.Name))
            {
                // Find the dataset in the database
                DataSet? dataSet = _context.DataSets.ToList().Where(ds => ds.Name == filter.Name).ToList()[0];

                // Add all the recordings that should be in this dataset,
                // since that doesn't come from the same table in the database
                List<DataStructure> recordingsInDataSet = _context.Recordings.Where(r => r.DataSetId == dataSet.Id).ToList();
                foreach (DataStructure recording in recordingsInDataSet)
                {
                    // Just double check that we didn't already add the recording
                    if (!dataSet.Recordings.Contains(recording))
                    {
                        dataSet.Recordings.Add(recording);
                    }
                }

                return dataSet;
            }
            throw new KeyNotFoundException();
        }

        public DataStructure AddRecording(DataStructure recording)
        {
            Debug.WriteLine("The AddRecording method has been called from the UDP receiver with data: " + recording.ToString());

            _context.Recordings.Add(recording);
            _context.SaveChanges();

            return recording;
        }

        public DataSet AddDataSet(DataSet dataSet)
        {
            _context.DataSets.Add(dataSet);
            _context.SaveChanges();
            return dataSet;
        }

        public void Update(DataStructure data)
        {
            DataStructure recording = GetRecordingById(data.DataSetId, data.Id);
            //_context.Entry(recording).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            _context.Recordings.Update(recording);
            _context.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        private void StartSensorReceiver()
        {
            //Start the UDP receiver on a new thread so it doesn't pause the program
            Task.Run(() => SensorReceiverUDP.Instance.RunReceiver());
            Debug.WriteLine("Sensor receiver started");
        }

        public override string ToString()
        {
            return $"Manage direkte data - Database";
        }
    }
}
