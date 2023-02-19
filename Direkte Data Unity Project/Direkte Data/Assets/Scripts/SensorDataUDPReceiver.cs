using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEditor.PackageManager;

public class SensorDataUDPReceiver : MonoBehaviour
{
    [Header("Debugging")]
    public bool DoDebug = false;
    public bool Running = false;

    [Header("Object Notifications")]
    public GameObject[] notifyObjects;
    public string methodToNotify;

    [Header("References")]
    [SerializeField] private Main _main;
    [SerializeField] private Direkte_DataSaver dataHandler;

    [Header("UDP Info")]
    [SerializeField] private string _ip = "192.168.0.104";
    [SerializeField] private int _portListen = 7001;
    [SerializeField] private int _portSend = 8001;

    private IPEndPoint _remoteEndPoint;
    private UdpClient _client;

    private Thread _receiveThread;

    private string received = "";

    private static char[] SPLITTERS = { ',' };

    public void Awake()
    {
        //Check if the ip address entered is valid. If not, sendMessage will broadcast to all ip addresses 
        IPAddress ip;
        if (IPAddress.TryParse(_ip, out ip))
        {
            _remoteEndPoint = new IPEndPoint(ip, _portSend);

        }
        else
        {
            _remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, _portSend);
        }

        //Initialize client and thread for receiving
        _client = new UdpClient(_portListen);

        _receiveThread = new Thread(new ThreadStart(ReceiveData));
        _receiveThread.IsBackground = true;
        _receiveThread.Start();
    }

    void Update()
    {
        //Check if a message has been recibed
        if (received != "")
        {
            Debug.Log("<color=cyan>UDPClient: message received \'" + received + "\'</color>");

            //Notify each object defined in the array with the message received
            foreach (GameObject g in notifyObjects)
            {
                g.SendMessage(methodToNotify, received, SendMessageOptions.DontRequireReceiver);
            }
            //Clear message
            received = "";
        }
    }

    //Call this method to send a message from this app to ipSend using portSend
    public void SendValue(string valueToSend)
    {
        try
        {
            if (valueToSend != "")
            {

                //Get bytes from string
                byte[] data = Encoding.UTF8.GetBytes(valueToSend);

                // Send bytes to remote client
                _client.Send(data, data.Length, _remoteEndPoint);
                Debug.Log("<color=yellow>UDPClient: send \'" + valueToSend + "\'</color>");
                //Clear message
                valueToSend = "";

            }
        }
        catch (Exception err)
        {
            Debug.LogError("Error udp send : " + err.Message);
        }
    }

    //This method checks if the app receives any message
    public void ReceiveData()
    {
        while (true)
        {
            try
            {
                Debug.Log("<color=lightgray>Waiting to receive data</color>");

                // Bytes received
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                var data = _client.Receive(ref anyIP);

                Debug.Log($"<color=cyan>Received data!</color>");

                // Bytes into text
                string text = "";
                text = Encoding.UTF8.GetString(data);

                Debug.Log($"<color=cyan>Data parsed to text: {text}</color>");

                OkNowParse(text);
                received = text;
            }
            catch (Exception err)
            {
                Debug.Log("Error:" + err.ToString());
            }
        }
    }

    private void OkNowParse(string rawText)
    {
        // Ignore the message if it's just the Hello World! one :)
        if (rawText == "Hello World!")
        { return; }

        // Expected format is "0,0,0,0"
        string[] datapoints = rawText.Split(SPLITTERS);

        // The first number is the time
        int time = int.Parse(datapoints[0]);

        // The next three numbers are the rotations on the axes
        string rotation = $"{datapoints[1]},{datapoints[2]},{datapoints[3]}";

        DataStructure recording = dataHandler.ParseRecordingToDataSet(time, rotation);

        Thread.Sleep((int)_main.UpdateTime*1000);
    }

    //Exit UDP client
    public void OnDisable()
    {
        if (_receiveThread != null)
        {
            _receiveThread.Abort();
            _receiveThread = null;
        }
        _client.Close();
        Debug.Log("<color=red>UDPClient: exit</color>");
    }
}
