import zsensor
from zsensor import Sensor

def fuel():
	charge = zsensor.Sensor("fuel_gauge")
	charge.measure()
	out = charge.get_float(zsensor.GAUGE_STATE_OF_CHARGE)
	print(out)