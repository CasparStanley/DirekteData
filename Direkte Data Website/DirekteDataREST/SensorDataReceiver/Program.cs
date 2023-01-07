// See https://aka.ms/new-console-template for more information
using SensorDataReceiver;

//FakeSensorReceiver fakeReceiver = new FakeSensorReceiver();
//fakeReceiver.StartReceiver();

SensorReceiverUDP sensorReceiver = new SensorReceiverUDP();
if (sensorReceiver.Started == false)
{
    sensorReceiver.Started = true;
    await sensorReceiver.StartReceiver();
}