from sense_hat import SenseHat
from socket import *
import time

s = SenseHat()
s.low_light = True

HOST = "192.168.0.102"
PORT = 7001
DATA = "Hello World!"

# 0.066 is 1/15th of a second, meaning it will update 15 times a second
UPDATE_FREQUENCY = 0.066

sensorTime = 0

# Create a socket to use internet and UDP
clientSocket = socket(AF_INET, SOCK_DGRAM)
clientSocket.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)
clientSocket.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)

#clientSocket.bind((HOST, PORT))

# Test message at the beginning:
clientSocket.sendto(DATA.encode(), (HOST, PORT))

# This update function runs with the speed of the UPDATE_FREQUENCY
def update(t):
  sensorTime = int(round(t, 0))

  orientation = s.get_orientation()
  pitch = int(round(orientation["pitch"], 0))
  roll = int(round(orientation["roll"], 0))
  yaw = int(round(orientation["yaw"], 0))
  
  print("time: {0} - pitch: {1}, roll: {2}, yaw: {3}".format(sensorTime, pitch, roll, yaw))
  
  DATA = ("{0},{1},{2},{3}".format(sensorTime, pitch, roll, yaw))
  
  # Finally, broadcast the data on the port
  clientSocket.sendto(DATA.encode(), (HOST, PORT))

# Initial screen update
update(0)
  
while True:
    update(sensorTime + UPDATE_FREQUENCY)

    sensorTime += UPDATE_FREQUENCY

    time.sleep(UPDATE_FREQUENCY)


