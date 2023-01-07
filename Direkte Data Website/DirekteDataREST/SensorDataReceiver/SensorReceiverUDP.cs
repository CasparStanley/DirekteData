using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using ModelLib;
using System.Reflection.Metadata;
using DirekteDataREST.Managers;
using SensorDataReceiver.Controllers;
using System.Runtime.CompilerServices;

namespace SensorDataReceiver
{
    internal class SensorReceiverUDP : ReceiverStructure
    {
        public override int Port { get; set; } = 7001;
        public bool Started = false;
        public bool Running = true;

        private static char[] SPLITTERS = { ',' };

        public override async Task StartReceiver()
        {
            // TODO: create a new DataSet to write to

            UdpClient client = new UdpClient(7001);
            IPEndPoint fromEP = new IPEndPoint(IPAddress.Loopback, Port);
            byte[] data;

            Console.WriteLine("Client: " + client.ToString());
            Console.WriteLine("IPEndPoint: " + IPAddress.Loopback);

            while (Running)
            {
                Thread.Sleep(1000);
                // Modtag data
                try
                {
                    Console.WriteLine("Attempting to receive some data from port: " + Port);

                    data = client.Receive(ref fromEP);

                    Console.WriteLine("Data received: " + data);

                    string str = Encoding.UTF8.GetString(data);

                    Console.WriteLine("Data converted: " + str);

                    // Ignore the message if it's just the Hello World! one :)
                    if (str == "Hello World!")
                    { continue; }

                    // Expected format is "0,0,0,0"
                    string[] datapoints = str.Split(SPLITTERS);

                    // The first number is the time
                    int time = int.Parse(datapoints[0]);
                    // The next three numbers are the rotations on the axes
                    string rotation = $"{datapoints[1]},{datapoints[2]},{datapoints[3]}";

                    DataStructure newSensorRecording = new DataStructure(time, rotation, 1);

                    // Send the data to the controller!
                    await SensorController.AddSensorDataAsync(newSensorRecording);

                    Console.WriteLine("Modtager: " + newSensorRecording.ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Data could not be converted, error: {ex}");
                }
            }
        }
    }
}
