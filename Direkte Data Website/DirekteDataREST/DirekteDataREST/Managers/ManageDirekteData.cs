using ModelLib;
using System;

namespace DirekteDataREST.Managers
{
    public class ManageDirekteData : IManageDirekteData
    {
        // Lazy Singleton
        private static readonly Lazy<ManageDirekteData> _instance = new Lazy<ManageDirekteData>(() => new ManageDirekteData());
        public static ManageDirekteData Instance { get { return _instance.Value; } }

        private static List<DataStructure> _mockRecordings = new()
        {
            new DataStructure(0, "0,0,0"),
            new DataStructure(1, "0,1,0"),
            new DataStructure(2, "0,5,0"),
            new DataStructure(3, "0,10,0"),
            new DataStructure(4, "0,15,0")
        };

        public ManageDirekteData()
        {
        }

        public DataStructure AddData(DataStructure data)
        {
            _mockRecordings.Add(data);
            return data;
        }

        public void DeleteItem(int id)
        {
            if (_mockRecordings[id] != null)
            {
                _mockRecordings.RemoveAt(id);
            }
        }

        public IEnumerable<DataStructure> GetAll()
        {
            return _mockRecordings;
        }

        public DataSet GetDataSetById(int id)
        {
            throw new NotImplementedException();
        }

        public DataStructure GetRecordingById(int dataSetId, int id)
        {
            return _mockRecordings[id];
        }

        public void Update(DataStructure data)
        {
            throw new NotImplementedException();
        }

        public void GenerateFakeSensorData()
        {
            int updateFrequency = 1;
            int fakeTime = 0;

            string fakeData = "0,0,0";

            bool running = true;

            while (running)
            {
                fakeTime += updateFrequency;

                int fakeX = new Random().Next(0, 359);
                int fakeY = new Random().Next(0, 359);
                int fakeZ = new Random().Next(0, 359);

                string fakeRotation = $"{fakeX},{fakeY},{fakeZ}";

                fakeData = $"{fakeTime},{fakeRotation}";

                Console.WriteLine(fakeData);

                DataStructure dataObj = new DataStructure(fakeTime, fakeRotation);

                AddData(dataObj);

                Thread.Sleep(updateFrequency * 1000);
            }
        }

        public override string ToString()
        {
            return $"Manage direkte data";
        }
    }
}