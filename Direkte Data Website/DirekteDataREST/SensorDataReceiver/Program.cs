// See https://aka.ms/new-console-template for more information
using SensorDataReceiver;

// HACK: WE USE THE FAKE RECEIVER RIGHT NOW
FakeSensorReceiver fakeReceiver = new FakeSensorReceiver();
fakeReceiver.StartReceiver();

//SensorReceiverUDP sensorReceiver = new SensorReceiverUDP();
//sensorReceiver.StartReceiver();