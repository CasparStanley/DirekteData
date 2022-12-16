﻿using ModelLib;
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
            new DataStructure(0, 0, "0,0,0"),
            new DataStructure(1, 1, "0,1,0"),
            new DataStructure(2, 2, "0,5,0"),
            new DataStructure(3, 4, "0,10,0"),
            new DataStructure(4, 10, "0,15,0")
        };
        public void AddData(DataStructure data)
        {
            _mockRecordings.Add(data);
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

                DataStructure dataObj = new DataStructure(fakeTime, 1, fakeRotation);

                AddData(dataObj);

                Thread.Sleep(updateFrequency * 1000);
            }
        }
    }
}