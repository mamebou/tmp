from machine import Pin
import zled
import time

ble_msg = ""
is_ble_connected = False

NUS_UUID = 'a72c3f53-ac6f-4367-8d65-c855ee89acee'
RX_UUID = '4836c2f5-001a-4d2b-a67f-a2701b1354e5'
TX_UUID = '93222d1f-2837-4f1d-88d0-e30b6d1935e1'

ble = ESP32_BLE("ESP32BLE")