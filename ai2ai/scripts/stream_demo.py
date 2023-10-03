import zsensor
import utime
import quaternion
from zsensor import Sensor

#Uncomment to disable screen when running this code
from machine import Pin

Screen_CS = Pin(("GPIO_EXP1", 6), Pin.OUT)
Screen_PWR = Pin(("GPIO_EXP1", 4), Pin.OUT)

Screen_PWR.off()
Screen_CS.on()


# Accelerometer
lis2dw = Sensor("accelerometer")
bno085 = Sensor("imu")

i = 1
jump = 0
gyro_result = True;

while True:
	try:
		gyro_result = True;
		lis2dw.measure()
		bno085.measure()
	except OSError:
		gyro_result = False;
        
	if(gyro_result):
		a_x = lis2dw.get_float(zsensor.ACCEL_X)
		a_y = lis2dw.get_float(zsensor.ACCEL_Y)
		a_z = lis2dw.get_float(zsensor.ACCEL_Z)    
        
		quat = bno085.get_quaternion(zsensor.ROTATION_VECTOR_XYZW)
		q_list = quaternion.list(quat)
		x = q_list[0]
		y = q_list[1]
		z = q_list[2]
		w = q_list[3]

		#snprintk(json, max_size,
		#"{\"X\":%.03f,\"Y\":%.03f,\"Z\":%.03f,\"W\":%.03f,\"jump\":%d,"
		#"\"AX\":%.03f,\"AY\":%.03f,\"AZ\":%.03f}",
		#q.x, q.y, q.z, q.w, is_jump(&acc), acc.x, acc.y, acc.z);
        
		buf = ("{\"X\":%.3f" %x + ",\"Y\":%.3f" %y + ",\"Z\":%.3f" % z +",\"W\":%.3f" %w +
		",\"jump\":" + str(jump) + ",\"AX\":%.3f" %a_x + ",\"AY\":%.3f" %a_y + ",\"AZ\":%.3f" %a_z +"}")
        
		print(buf)
        
		utime.sleep_ms(50)