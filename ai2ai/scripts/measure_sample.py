import zsensor
import utime
import quaternion
from zsensor import Sensor
from machine import Pin
acc_sensor = Sensor("accelerometer")
bno085 = Sensor('imu')
touch = []

color = [0, 0, 255]
touched = []

def touchIndex(pin):
    try:
        return touch.index(pin)
    except ValueError:
        return -1
def cb(p):
    i = touchIndex(p)
    if i in touched:
        touched.remove(i)
    else:
        touched.append(i)
for i in range(5):
    touch.append(Pin(("T0", i)))
    touch[i].irq(cb, Pin.IRQ_RISING)
while True:
    try:
        bno085.measure()
        acc_sensor.measure()
    except OSError:
        print("OSError")
    a_x = acc_sensor.get_float(zsensor.ACCEL_X)
    a_y = acc_sensor.get_float(zsensor.ACCEL_Y)
    a_z = acc_sensor.get_float(zsensor.ACCEL_Z)
    quat = bno085.get_quaternion(zsensor.ROTATION_VECTOR_XYZW)
    q_list = quaternion.list(quat)
    x = q_list[0]
    y = q_list[1]
    z = q_list[2]
    w = q_list[3]
    buf = ("%.3f" %x + " %.3f" %y + " %.3f" %z +" %.3f" %w + " %.3f" %a_x + " %.3f" %a_y + " %.3f" %a_z + " %d" %len(touched))
    print(buf, end="")
    utime.sleep_ms(50)