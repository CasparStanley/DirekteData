﻿using ModelLib;
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

        public void Update(DataStructure data)
        {
            DataStructure recording = GetRecordingById(data.DataSetId, data.Id);
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
