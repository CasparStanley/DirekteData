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

namespace SensorDataReceiver
{
    internal class SensorReceiverUDP : ReceiverStructure
    {
        public override int Port { get; set; } = 7001;

        private static char[] SPLITTERS = { ',' };

        public override void StartReceiver()
        {
            UdpClient client = new UdpClient(Port);
            IPEndPoint fromEP = new IPEndPoint(IPAddress.Loopback, Port);

            // Modtage
            try
            {
                byte[] data = client.Receive(ref fromEP);
                string str = Encoding.UTF8.GetString(data);

                // Expected format is "0,0,0,0"
                string[] datapoints = str.Split(SPLITTERS);

                // The first number is the time
                int time = int.Parse(datapoints[0]);
                // The next three numbers are the rotations on the axes
                string rotation = $"{datapoints[1]},{datapoints[2]},{datapoints[3]}";

                DataStructure dataObj = new DataStructure(time, rotation);

                Console.WriteLine("Modtager: " + str);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Data could not be converted, error: {ex}");
            }
        }
    }
}
