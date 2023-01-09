using ModelLib;
using System.Net.Sockets;
using System.Net;
using System.Text;
using DirekteDataREST.Controllers;
using DirekteDataREST.Managers;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Globalization;

namespace DirekteDataREST.SensorReceiver
{
    public class SensorReceiverUDP : ReceiverStructure
    {
        // Lazy Singleton
        private static readonly Lazy<SensorReceiverUDP> _instance = new Lazy<SensorReceiverUDP>(() => new SensorReceiverUDP());
        public static SensorReceiverUDP Instance { get { return _instance.Value; } }

        // https://direktedatarest2022.azurewebsites.net/api/DirekteData
        // http://localhost:5290/api/DirekteData

        private const string RESTURL = "https://direktedatarest2022.azurewebsites.net/api/DirekteData";
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
                    // Make sure that '.' is used as a decimal separator
                    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    ci.NumberFormat.CurrencyDecimalSeparator = ".";

                    float time = float.Parse(datapoints[0], NumberStyles.Any, ci);
                    Debug.WriteLine("\n\nTime parsed to: " + time + "\n\n");

                    // The next three numbers are the rotations on the axes
                    string rotation = $"{datapoints[1]},{datapoints[2]},{datapoints[3]}";
                    Debug.WriteLine("\n\nRotation parsed to: " + rotation + "\n\n");

                    // Create the new recording object with the recorded values!
                    DataStructure newSensorRecording = new DataStructure(time, rotation, LiveDataHolder.SelectedDataSetId);

                    // Convert the Recording to a json and send it as an http post request to the controller
                    var jsonDataSet = JsonConvert.SerializeObject(newSensorRecording);
                    HttpResponseMessage response = await SendHttpPostRequest(jsonDataSet, POST_RECORDING);
                    Debug.WriteLine("\n\n" + response + "\n\n");
                    Debug.WriteLine("\n\n" + response.Content + "\n\n");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Data could not be converted, error: {ex}");
                }
            }
        }

        private void ReceiverStartup()
        {
            //// Since this recording has just started, create a new DataSet that we can link these recordings to
            //string dataSetName = $"{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToShortTimeString()}";
            //string dataSetDesc = $"This DataSet was recorded on {DateTime.Now.ToShortDateString()} at {DateTime.Now.ToShortTimeString()}";

            //// Convert the DataSet to a json and send it as an http post request to the controller
            //var jsonDataSet = JsonConvert.SerializeObject(new DataSet(dataSetName, dataSetDesc, new List<DataStructure>()));
            //HttpResponseMessage response = await SendHttpPostRequest(jsonDataSet, POST_DATASET);
            //Debug.WriteLine("\n\n" + response + "\n\n");
            //Debug.WriteLine("\n\n" + response.Content.ReadAsStringAsync().Result + "\n\n");

            //DataSet createdDataSet = JsonConvert.DeserializeObject<DataSet>(response.Content.ReadAsStringAsync().Result);
            //SelectedDataSetId = GetDataSet(createdDataSet.Name).Id;

            //Debug.WriteLine("\n\nDataSet " + SelectedDataSetId + ") " + createdDataSet.Name + " was created\n\n");

            Running = true;

            client = new UdpClient(Port);
            fromEP = new IPEndPoint(IPAddress.Loopback, Port);

            Debug.WriteLine("\n\nClient: " + client.ToString());
            Debug.WriteLine("IPEndPoint: " + IPAddress.Loopback);
            Debug.WriteLine("");
            Debug.WriteLine("");
        }

        private async Task<HttpResponseMessage> SendHttpPostRequest(string json, string postAttribute)
        {
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine("\nJSON\n" + json + "\n\n");

                return await client.PostAsync(RESTURL + postAttribute, content);
            }
        }

        private async Task<HttpResponseMessage> GetDataSet(string name)
        {
            using (var client = new HttpClient())
            {
                string escapedString = Uri.EscapeDataString(name);

                Debug.WriteLine($"\nSearching for DataSet with name: {name}\nEscaped string: {escapedString}\n\n");

                return await client.GetAsync(RESTURL + "/search?Name=" + escapedString);
            }
        }
    }
}