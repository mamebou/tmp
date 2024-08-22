import asyncio
from bleak import BleakScanner, BleakClient
from imu_ekf import *
import numpy as np
import time
from paho.mqtt import client as mqtt_client
import random
from Quaternion import Quat

broker = 'test.mosquitto.org'
port = 1883
topic = "mytopic/mqtt"
test_topic = "mytopic/test"
# generate client ID with pub prefix randomly
client_id = f'python-mqtt-{random.randint(0, 1000)}'
# username = 'emqx'
# password = 'public'

def connect_mqtt():
    def on_connect(client, userdata, flags, rc):
        if rc == 0:
            print("Connected to MQTT Broker!")
        else:
            print("Failed to connect, return code %d\n", rc)
    client = mqtt_client.Client(client_id)
    #client.username_pw_set(username, password)
    client.on_connect = on_connect
    client.connect(broker, port)
    return client

#for ekf
def calc_u(gyro, dt):
    gyro = np.array([
        [gyro[0]],
        [gyro[1]],
        [gyro[2]]
    ])
    u = gyro * dt
    return u

def calc_z(acc):
    z = np.array([
        [np.arctan(acc[1]/acc[2])], 
        [-np.arctan(acc[0]/np.sqrt(acc[1]**2+acc[2]**2))]
        ])
    return z

def convert_euler_to_Rxyz(x):
    c1 = np.cos(x[0][0])
    s1 = np.sin(x[0][0])
    c2 = np.cos(x[1][0])
    s2 = np.sin(x[1][0])
    c3 = np.cos(x[2][0])
    s3 = np.sin(x[2][0])
    Rx = np.array([
        [1, 0, 0],
        [0, c1, -s1],
        [0, s1, c1],
    ])
    Ry = np.array([
        [c2, 0, s2],
        [0, 1, 0],
        [-s2, 0, c2],
    ])
    Rz = np.array([
        [c3, -s3, 0],
        [s3, c3, 0],
        [0, 0, 1],
    ])
    Rxyz = Rz @ Ry @ Rx
    return Rxyz

# ESP32のデバイスを識別するためのUUID (これはデバイスにより異なります) 

# Nordic UART Service (NUS)
NUS_UUID = '6e400002-b5a3-f393-e0a9-e50e24dcca9e'
RX_UUID = '93222d1f-2837-4f1d-88d0-e30b6d1935e1'  # RX Characteristic UUID (from ESP32 to Computer)

#grobal variable
acc = None
ts_pre = None
dt = 0.01
m_client = None
count = 1
calib = [0, 0, 0]
sum = [0, 0, 0]
isCalib = False

# ekf init
x = np.array([[0], [0], [0]])
P = np.diag([1.74E-2*dt**2, 1.74E-2*dt**2, 1.74E-2*dt**2])

# コールバック関数: データが送信されたときに呼び出されます
def notification_handler(sender: int, data: bytearray, **_kwargs):
        global ts_pre
        global x
        global P
        global topic
        global m_client
        global calib
        global sum
        global count
        global isCalib
        imu_data = data.decode().split()
        gyro = np.array([float(imu_data[0]), float(imu_data[1]), float(imu_data[2]), float(imu_data[3])])
        w = gyro[3]
        x = gyro[0]
        y = gyro[1]
        z = gyro[2]
        a = np.sqrt(w**2 + x**2 + y**2 + z**2)
        data = [y/a, x/a, z/a, w/a]
        q = Quat(data)
        rpy = [q.dec,-q.roll,q.ra]
        for i in range(3):
            if  rpy[i-1] > 180:
                rpy[i-1] -= 360
            elif rpy[i-1] <  -180:
                rpy[i-1] += 360
        msg = str(rpy[0]) + " " + str(rpy[1]) + " " + str(rpy[2])
        print(msg)
        m_client.publish(topic, msg)

async def run():
    global m_client
    m_client = connect_mqtt()
    m_client.loop_start()
    # 1. 周囲のBLE発信をスキャン
    scanner = BleakScanner()
    devices = await scanner.discover()

    clients = []
    for device in devices:
        if device.name == 'PALL0':
            #print(f'name:{device.name},address:{device.address}')
            client = BleakClient(device)
            clients.append(client)

    try:
        # 2. クライアント（ESP32などのデバイス）とデータのやり取りをする
        print(clients)
        for client in clients:
            await client.connect()
            # Characteristicの情報を得るために記述。本番ではコメントアウトしても良い
            #for service in client.services:
                #print('---------------------')
                #print(f"service uuid:{service.uuid}, description:{service.description}")
                #[print(f'{c.properties},{c.uuid}') for c in service.characteristics]
            
            await client.start_notify('6e400003-b5a3-f393-e0a9-e50e24dcca9e', notification_handler)
            
        while True:
            await asyncio.sleep(1.0)
    finally:
        print(14)
        # for client in clients:
        #     await client.stop_notify(RX_UUID)
        #     await client.disconnect()

asyncio.run(run())
