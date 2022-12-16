#https://trinket.io/library/trinkets/72d4b99d05
from sense_hat import SenseHat
import socket
import time
import random

s = SenseHat()
s.low_light = True

HOST = '127.0.0.1'
PORT = 7001

UPDATE_FREQUENCY = 1

sensorTime = 0

sock = socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind((HOST, PORT))
#sock.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)

data = ''

def update(t):
  sensorTime = t

  orientation = s.get_orientation()
  pitch = round(orientation["pitch"], 0)
  roll = round(orientation["roll"], 0)
  yaw = round(orientation["yaw"], 0)
  
  print("time: {0} - pitch: {1}, roll: {2}, yaw: {3}".format(sensorTime, pitch, roll, yaw))
  
  data = ("{0},{1},{2},{3}".format(sensorTime, pitch, roll, yaw))
  
  send_udp(data)

def send_udp(d):
  sock.sendto(d, (HOST, PORT))

# Initial screen update
update(0)
  
while True:
    update(sensorTime + UPDATE_FREQUENCY)

    sensorTime += UPDATE_FREQUENCY

    time.sleep(UPDATE_FREQUENCY)