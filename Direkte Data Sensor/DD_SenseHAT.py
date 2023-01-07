#https://trinket.io/library/trinkets/72d4b99d05
from sense_hat import SenseHat
from socket import *
import time

s = SenseHat()
s.low_light = True

HOST = "192.168.0.102"
PORT = 7001
DATA = "Hello World!"

UPDATE_FREQUENCY = 1

sensorTime = 0

clientSocket = socket(AF_INET, SOCK_DGRAM)
clientSocket.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)
clientSocket.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)

#clientSocket.bind((HOST, PORT))

# Test message at the beginning:
clientSocket.sendto(DATA.encode(), (HOST, PORT))

def update(t):
  sensorTime = t

  orientation = s.get_orientation()
  pitch = round(orientation["pitch"], 0)
  roll = round(orientation["roll"], 0)
  yaw = round(orientation["yaw"], 0)
  
  print("time: {0} - pitch: {1}, roll: {2}, yaw: {3}".format(sensorTime, pitch, roll, yaw))
  
  DATA = ("{0},{1},{2},{3}".format(sensorTime, pitch, roll, yaw))
  
  clientSocket.sendto(DATA.encode(), (HOST, PORT))

# Initial screen update
update(0)
  
while True:
    update(sensorTime + UPDATE_FREQUENCY)

    sensorTime += UPDATE_FREQUENCY

    time.sleep(UPDATE_FREQUENCY)