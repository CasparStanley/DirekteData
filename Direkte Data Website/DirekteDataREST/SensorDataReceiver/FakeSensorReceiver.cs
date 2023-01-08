using ModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorDataReceiver
{
    internal class FakeSensorReceiver : ReceiverStructure
    {
        public override int Port { get; set; } = 7001;

        private int updateFrequency = 1;
        private int fakeTime = 0;

        private string fakeData = "0,0,0";

        private bool running = true;

        public override async Task StartReceiver()
        {
            //IManageDirekteData mgr = ManageDirekteData.Instance;

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

                //mgr.AddData(dataObj);

                Thread.Sleep(updateFrequency * 1000);
            }
        }
    }
}
