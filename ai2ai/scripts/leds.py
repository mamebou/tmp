# title: leds.py
# author: Ai2Ai Oy
# -> to be uploaded into a PALL0 containing mcumgr and SMP enabled micropython

import zled

led = zled.Led()
color = [255, 0, 0]
led.all_color(color)
led.all_on()
while True:
    print("hello")
