import asyncio
from bleak import BleakScanner, BleakClient
from imu_ekf import *
import time
from paho.mqtt import client as mqtt_client
import random
import math
import numpy as np



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


def normalize(v):
    norm = np.linalg.norm(v)
    if norm == 0:
        return v
    return v / norm

def madgwick_filter(accel, gyro, beta):
    q = np.array([1.0, 0.0, 0.0, 0.0]) # 初期のクォータニオン
    sample_period = 1.0 / 100.0 # サンプリング周期 (100 Hzの場合)

    for i in range(len(accel)):
        accel[i] = normalize(accel[i])
        gyro[i] = normalize(gyro[i])

        q0, q1, q2, q3 = q
        ax, ay, az = accel[i]
        gx, gy, gz = gyro[i]

        s0 = 2.0 * (q1 * q3 - q0 * q2)
        s1 = 2.0 * (q0 * q1 + q2 * q3)
        s2 = q0**2 - q1**2 - q2**2 + q3**2
        s3 = 2.0 * (q1 * q2 - q0 * q3)

        q_dot1 = 0.5 * (-s1 * gx - s2 * gy - s3 * gz)
        q_dot2 = 0.5 * (s0 * gx + s2 * gz - s3 * gy)
        q_dot3 = 0.5 * (s0 * gy - s1 * gz + s3 * gx)
        q_dot4 = 0.5 * (s0 * gz + s1 * gy - s2 * gx)

        q0 += q_dot1 * sample_period
        q1 += q_dot2 * sample_period
        q2 += q_dot3 * sample_period
        q3 += q_dot4 * sample_period

        norm = math.sqrt(q0**2 + q1**2 + q2**2 + q3**2)
        q = np.array([q0, q1, q2, q3]) / norm
    roll = math.atan2(2.0 * (q0 * q1 + q2 * q3), 1 - 2 * (q1**2 + q2**2))
    pitch = math.asin(2.0 * (q0 * q2 - q3 * q1))
    yaw = math.atan2(2.0 * (q0 * q3 + q1 * q2), 1 - 2 * (q2**2 + q3**2))
    return roll, pitch, yaw




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
m6a0 = np.zeros((3,))
m6g0 = np.zeros((3,))
x0 = np.zeros((2,))
isInitial = False
x = x0
c = 0
b = 0
r = 0
cgy = 0
P = 9
Ts = 0
Yaw = 0
Tri = 0
beta = 0.041

# ekf init
x = np.array([[0], [0], [0]])
P = np.diag([1.74E-2*dt**2, 1.74E-2*dt**2, 1.74E-2*dt**2])
alpha = 0.98  # ジャイロスコープの重み
dt = 0.01  # サンプリング間隔（秒）
q = np.array([1.0, 0.0, 0.0, 0.0])  # 初期クォータニオン [w, x, y, z]

# コールバック関数: データが送信されたときに呼び出されます
def notification_handler(sender: int, data: bytearray, **_kwargs):
    global alpha
    global dt
    global q
    imu_data = data.decode().split()
    msg = data.decode()
    m_client.publish(topic, msg)
    print(msg)




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