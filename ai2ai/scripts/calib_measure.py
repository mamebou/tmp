import zsensor
import utime
import quaternion
from zsensor import Sensor
from machine import Pin
acc_sensor = Sensor("accelerometer")
bno085 = Sensor('imu')
touch = []
calibrate = False

color = [0, 0, 255]
touched = []
c_ax = 0
c_ay = 0
c_az = 0
c_gx = 0
c_gy = 0
c_gz = 0
sum_ax = 0
sum_ay = 0
sum_az = 0
sum_gx = 0
sum_gy = 0
sum_gz = 0
count = 0





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
    
    if calibrate:
        a_x = acc_sensor.get_float(zsensor.ACCEL_X) - c_ax
        a_y = acc_sensor.get_float(zsensor.ACCEL_Y) - c_ay
        a_z = acc_sensor.get_float(zsensor.ACCEL_Z) - c_az
        quat = bno085.get_quaternion(zsensor.ROTATION_VECTOR_XYZW)
        q_list = quaternion.list(quat)
        x = q_list[0] - c_gx
        y = q_list[1] - c_gy
        z = q_list[2] - c_gz
        w = q_list[3]
        buf = ("%.3f" %x + " %.3f" %y + " %.3f" %z +" %.3f" %w + " %.3f" %a_x + " %.3f" %a_y + " %.3f" %a_z + " %d" %len(touched))
        print(buf, end="")
        utime.sleep_ms(50)
    else:
        if count == 99:
            c_ax = sum_ax / 100
            c_ay = sum_ay / 100
            c_az = sum_az / 100
            c_gx = sum_gx / 100
            c_gy = sum_gy / 100
            c_gz = sum_gz / 100
            calibrate = True
        else:
            a_x = acc_sensor.get_float(zsensor.ACCEL_X)
            a_y = acc_sensor.get_float(zsensor.ACCEL_Y)
            a_z = acc_sensor.get_float(zsensor.ACCEL_Z)
            quat = bno085.get_quaternion(zsensor.ROTATION_VECTOR_XYZW)
            q_list = quaternion.list(quat)
            x = q_list[0]
            y = q_list[1]
            z = q_list[2]
            w = q_list[3]
            sum_ax = sum_ax + a_x
            sum_ay = sum_ay + a_y
            sum_az = sum_az + a_z
            sum_gx = sum_gx + x
            sum_gy = sum_gy + y
            sum_gz = sum_gz + z
            count = count + 1
        utime.sleep_ms(50)

