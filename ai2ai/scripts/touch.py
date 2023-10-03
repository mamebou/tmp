from machine import Pin
import zled
import time

led = zled.Led()
led.all_off()
color = [0, 0, 255]
led.all_color(color)
pin = 0

touch = []
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
    #print("Index %d" % i)
    led.toggle(5 + i)

for i in range(5):
    touch.append(Pin(("T0", i)))
    touch[i].irq(cb, Pin.IRQ_RISING)

while (True):
    time.sleep_ms(1000)
    print(touched)
