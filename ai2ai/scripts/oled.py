from machine import Pin

Screen_CS = Pin(("GPIO_EXP1", 6), Pin.OUT)
Screen_PWR = Pin(("GPIO_EXP1", 4), Pin.OUT)

def screen(val):
	if(val):
		Screen_CS.off()
		Screen_PWR.on()
	else:
 		Screen_PWR.off()
		Screen_CS.on()