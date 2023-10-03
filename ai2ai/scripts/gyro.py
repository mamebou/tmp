import zsensor
import utime
import quaternion
from zsensor import Sensor

bno085 = Sensor('imu')
again = True

while again:
	try:
		again = False
		bno085.measure()
	except OSError:
		again = True
		utime.sleep(1)

utime.sleep(0.5)        
while True:
	bno085.measure()
	quat = bno085.get_quaternion(zsensor.ROTATION_VECTOR_XYZW)
	q_list = quaternion.list(quat)
	print(q_list)
	print("this is gyro data")
	utime.sleep(0.5)