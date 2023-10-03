import zsensor
from zsensor import Sensor

# Accelerometer
lis2dw = Sensor("accelerometer")
lis2dw.measure()
lis2dw.get_float(zsensor.ACCEL_X)
lis2dw.get_float(zsensor.ACCEL_Y)
lis2dw.get_float(zsensor.ACCEL_Z)

# Barometer
lps33w = Sensor("barometer")
lps33w.measure()
lps33w.get_float(zsensor.AMBIENT_TEMP)

# fuel-gauge -> charge of battery
charge = zsensor.Sensor("fuel_gauge")
charge.measure()
charge.get_float(zsensor.GAUGE_STATE_OF_CHARGE)

# Magnetic hall effect sensors - not tested for new version
#si7210_0 = Sensor("SI7210_0")
#si7210_0.measure()
#si7210_0.get_float(zsensor.AMBIENT_TEMP)
#si7210_0.get_float(zsensor.MAGN_Z)
#
#si7210_1 = Sensor("SI7210_1")
#si7210_1.measure()
#si7210_1.get_float(zsensor.AMBIENT_TEMP)
#si7210_1.get_float(zsensor.MAGN_Z)
#
#si7210_2 = Sensor("SI7210_2")
#si7210_2.measure()
#si7210_2.get_float(zsensor.AMBIENT_TEMP)
#si7210_2.get_float(zsensor.MAGN_Z)
#
#si7210_3 = Sensor("SI7210_3")
#si7210_3.measure()
#si7210_3.get_float(zsensor.AMBIENT_TEMP)
#si7210_3.get_float(zsensor.MAGN_Z)
#
#si7210_4 = Sensor("SI7210_4")
#si7210_4.measure()
#si7210_4.get_float(zsensor.AMBIENT_TEMP)
#si7210_4.get_float(zsensor.MAGN_Z)
#
#si7210_5 = Sensor("SI7210_5")
#si7210_5.measure()
#si7210_5.get_float(zsensor.AMBIENT_TEMP)
#si7210_5.get_float(zsensor.MAGN_Z)
#
#si7210_6 = Sensor("SI7210_6")
#si7210_6.measure()
#si7210_6.get_float(zsensor.AMBIENT_TEMP)
#si7210_6.get_float(zsensor.MAGN_Z)
#
#si7210_7 = Sensor("SI7210_7")
#si7210_7.measure()
#si7210_7.get_float(zsensor.AMBIENT_TEMP)
#si7210_7.get_float(zsensor.MAGN_Z)
#
#si7210_8 = Sensor("SI7210_8")
#si7210_8.measure()
#si7210_8.get_float(zsensor.AMBIENT_TEMP)
#si7210_8.get_float(zsensor.MAGN_Z)
#
#si7210_9 = Sensor("SI7210_9")
#si7210_9.measure()
#si7210_9.get_float(zsensor.AMBIENT_TEMP)
#si7210_9.get_float(zsensor.MAGN_Z)
#
#si7210_10 = Sensor("SI7210_10")
#si7210_10.measure()
#si7210_10.get_float(zsensor.AMBIENT_TEMP)
#si7210_10.get_float(zsensor.MAGN_Z)
#
#si7210_11 = Sensor("SI7210_11")
#si7210_11.measure()
#si7210_11.get_float(zsensor.AMBIENT_TEMP)
#si7210_11.get_float(zsensor.MAGN_Z)
