using ModelLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SensorDataReceiver.Controllers
{
    public class SensorController
    {
        private const string RESTURL = "https://direktedatarest2022.azurewebsites.net/api/DirekteData";

        public SensorController()
        {
        }

        public static async Task AddSensorDataAsync(DataStructure newSensorRecording)
        {
            DataStructure testRecording = new DataStructure(60, 1, "90,80,70", 2);

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync( RESTURL, new StringContent(JsonConvert.SerializeObject(testRecording), Encoding.UTF8, "application/json"));
            }
        }
    }
}
