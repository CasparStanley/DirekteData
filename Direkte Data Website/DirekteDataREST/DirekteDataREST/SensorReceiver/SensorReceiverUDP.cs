using ModelLib;
using System.Net.Sockets;
using System.Net;
using System.Text;
using DirekteDataREST.Controllers;
using DirekteDataREST.Managers;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DirekteDataREST.SensorReceiver
{
    public class SensorReceiverUDP : ReceiverStructure
    {
        // Lazy Singleton
        private static readonly Lazy<SensorReceiverUDP> _instance = new Lazy<SensorReceiverUDP>(() => new SensorReceiverUDP());
        public static SensorReceiverUDP Instance { get { return _instance.Value; } }

        // http://direktedatarest2022.azurewebsites.net/api/DirekteData
        // http://localhost:5290/api/DirekteData

        private const string RESTURL = "http://direktedatarest2022.azurewebsites.net/api/DirekteData";
        private const string POST_RECORDING = "/AddRecording";
        private const string POST_DATASET = "/AddDataSet";

        public override int Port { get; set; } = 7001;
        public bool Running = false;

        private static char[] SPLITTERS = { ',' };

        // 20 miliseconds is 0,02 seconds
        // we should receive data once every 0,066 seconds so let's check more often to be safe
        private int updateTime = 20;

        private UdpClient client;
        private IPEndPoint fromEP;
        private byte[] data;

        private int currentDataSetId;

        public SensorReceiverUDP() { }

        public override async void RunReceiver()
        {
            // If the receiver hasn't been started yet, run the startup method
            if (!Running)
            {
                ReceiverStartup();
            }

            while (Running)
            {
                Thread.Sleep(updateTime);
                // Modtag data
                try
                {
                    Debug.WriteLine("Attempting to receive some data from port: " + Port);

                    data = client.Receive(ref fromEP);

                    Debug.WriteLine("Data received: " + data);

                    string str = Encoding.UTF8.GetString(data);

                    Debug.WriteLine("Data converted: " + str);

                    // Ignore the message if it's just the Hello World! one :)
                    if (str == "Hello World!")
                    { continue; }

                    // Expected format is "0,0,0,0"
                    string[] datapoints = str.Split(SPLITTERS);

                    // The first number is the time
                    int time = int.Parse(datapoints[0]);
                    // The next three numbers are the rotations on the axes
                    string rotation = $"{datapoints[1]},{datapoints[2]},{datapoints[3]}";

                    // Create the new recording object with the recorded values!
                    DataStructure newSensorRecording = new DataStructure(time, rotation, currentDataSetId);

                    // Send the data to the manager!
                    //mgr.AddRecording(newSensorRecording);

                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(RESTURL + POST_RECORDING, 
                            new StringContent(JsonConvert.SerializeObject(newSensorRecording), 
                            Encoding.UTF8, 
                            "application/json"));
                    }

                    Debug.WriteLine("Modtager: " + newSensorRecording.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Data could not be converted, error: {ex}");
                }
            }
        }

        private async void ReceiverStartup()
        {
            // Since this recording has just started, create a new DataSet that we can link these recordings to
            string dataSetName = DateTime.Now.ToShortDateString();
            string dataSetDesc = $"This DataSet was recorded on {DateTime.Now.ToShortDateString()} at {DateTime.Now.ToShortTimeString()}";

            // This creates the new DataSet and once returned it sets the currentDataSetId int
            // so we can add that to all future recordings
            //currentDataSetId = mgr.AddDataSet().Id;

            using (var client = new HttpClient())
            {
                var jsonDataSet = JsonConvert.SerializeObject(new DataSet(dataSetName, dataSetDesc, new List<DataStructure>()));
                StringContent content = new StringContent(jsonDataSet, Encoding.UTF8, "application/json");

                Debug.WriteLine("\nJSON\n" + jsonDataSet + "\n\n");

                var response = await client.PostAsync(RESTURL + POST_DATASET, content);

                Debug.WriteLine("\n\n" + response + "\n\n");
                // TODO: Get the DataSet id from the response
            }

            Running = true;

            client = new UdpClient(7001);
            fromEP = new IPEndPoint(IPAddress.Loopback, Port);

            Debug.WriteLine("\n\nClient: " + client.ToString());
            Debug.WriteLine("IPEndPoint: " + IPAddress.Loopback);
            Debug.WriteLine("");
            Debug.WriteLine("");
        }
    }
}