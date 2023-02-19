using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Threading;
using UnityEditor.PackageManager;

public class SensorDataUDPReceiver : MonoBehaviour
{
    [Header("Debugging")]
    public bool DoDebug = false;
    public bool Running = false;

    [Header("References")]
    [SerializeField] private Main _main;
    [SerializeField] private Direkte_DataSaver dataHandler;

    [Header("Raspberry Pi Info")]
    [SerializeField] private string _ip = "192.168.0.104";
    [SerializeField] private int _port = 7001;

    [Header("Receiver Info (Unity)")]
    [SerializeField] private int _returnPort = 8001;

    private readonly Queue<string> incomingQueue = new Queue<string>();
    static readonly object lockObject = new object();

    private IPEndPoint fromEP;
    //private UdpClient _client;

    private static char[] SPLITTERS = { ',' };

    Thread receiveThread;

    void Start()
    {
        StartReceiver();
    }
    public void StartReceiver()
    {
        //fromEP = new IPEndPoint(IPAddress.Any, 0);
        //_client = new UdpClient(_port);

        //_client.Client.Bind(fromEP);

        //Task.Run(async () => ReceivedUDPPacket());
        ReceivedUDPPacket();
        //StartReceiverThread();

        Debug.Log("UDP - Start Receiving..");
    }

    //private void StartReceiverThread()
    //{
    //    receiveThread = new Thread(() => ReceivedUDPPacket());
    //    receiveThread.IsBackground = true;
    //    Running = true;

    //    if (!Running) Running = true;

    //    receiveThread.Start();
    //}

    private void ReceivedUDPPacket()
    {
        UdpClient client = new UdpClient(_port);
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        Debug.Log("<color=yellow>Client and IpEndpoint created</color>");

        //using (var client = new UdpClient(_port))
        //{
        while (true)
        {
            // Modtag data
            try
            {
                Debug.Log("<color=yellow>Getting ready to receive from client</color>");

                //client.Connect(RemoteIpEndPoint);
                //var udpReceiveResult = await client.ReceiveAsync();
                Byte[] udpReceiveResult = client.Receive(ref RemoteIpEndPoint);
                //string str = Encoding.UTF8.GetString(udpReceiveResult.Buffer);
                string str = Encoding.UTF8.GetString(udpReceiveResult);

                Debug.Log("<color=yellow>RECEIVING FROM RASPBERRY PI!</color>");
                Debug.Log($"<color=orange>DATAPOINTS: {str}</color>");

                //string str = Encoding.UTF8.GetString(data);

                lock (lockObject)
                {

                    // Ignore the message if it's just the Hello World! one :)
                    if (str == "Hello World!")
                    { continue; }

                    // Expected format is "0,0,0,0"
                    string[] datapoints = str.Split(SPLITTERS);

                    // The first number is the time
                    int time = int.Parse(datapoints[0]);

                    // The next three numbers are the rotations on the axes
                    string rotation = $"{datapoints[1]},{datapoints[2]},{datapoints[3]}";

                    DataStructure recording = dataHandler.ParseRecordingToDataSet(time, rotation);
                }

            }
            catch (SocketException e)
            {
                // 10004 thrown when socket is closed
                if (e.ErrorCode != 10004)
                {
                    Debug.LogError("Socket exception while receiving data from udp client: " + e.Message);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error receiving data from udp client: {e.Message}");
            }

            Thread.Sleep((int)_main.UpdateTime*1000);
        }
        //}
    }

    //private void OnDestroy()
    //{
    //    Debug.Log("Sensor Data UDP Receiver was destroyed, shutting down client");
    //    Stop();
    //}

    //public void Stop()
    //{
    //    if (_client != null)
    //    {
    //        _client.Close();
    //        _client = null;
    //    }
    //    if (receiveThread!= null) 
    //    { 
    //        if (receiveThread.Join(100))
    //        {
    //            Debug.Log("UDP thread has terminated successfully");
    //        }
    //        else
    //        {
    //            Debug.Log("UDP thread did not terminate within 100ms, forcefully aborting");
    //            receiveThread.Abort();
    //        }

    //        Running = false;
    //    }
    //}
}
