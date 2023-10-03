# Jumping game: one-player, jump and leds get lit up

import math
import utime
import zled
import zsensor
import audio

from zsensor import Sensor
from machine import Pin

player = audio.Audio()
accel = Sensor("accelerometer")

lightAmount = None
i = None
walking = None
walkAmount = None
steps = None
lightUp = None
j = None
k = None

led = zled.Led()
VIBRO = Pin(("GPIO_EXP1", 2 or 3), Pin.OUT)
VIBRO.off()

# A generator functions that yield values from start to stop in increments of step
def upRange(start, stop, step):
  while start <= stop:
    yield start
    start += abs(step)
def downRange(start, stop, step):
  while start >= stop:
    yield start
    start -= abs(step)

# Activates and sets the LEDs to a color (lightAmount) value 
# The LEDs light up in a gradient pattern
def lightBall(lightAmount):
  global i, walking, walkAmount, steps, lightUp, j, k
  for i in (0 <= float(lightAmount)) and upRange(0, float(lightAmount), 1) or downRange(0, float(lightAmount), 1):
    led.color(i, ((102,255,255)))
    led.on(i)

# Describe this function...
def walk():
  global lightAmount, i, walking, walkAmount, steps, lightUp, j, k
  led.all_off()
  walking = 1
  walkAmount = 20 # finishes at "55 steps", led lights up every 'walkAmount' steps
  steps = 0
  while walking:
    accel.measure()
    z_axis = 0
    z_axis = accel.get_float(zsensor.ACCEL_Z)

    utime.sleep(0.01)

    accel.measure()
    z_axis1 = 0
    z_axis1=accel.get_float(zsensor.ACCEL_Z)
    z_result = z_axis - z_axis1

    if z_result > 15 or z_result < -15:
      steps = (steps if isinstance(steps, int) else 0) + 1 # aka steps += 1 (if steps is integer)
      print("steps " + str(steps))

    lightUp = round(steps % walkAmount)

    if steps > 1 and not lightUp:
      lightBall(round(steps / walkAmount) - 1)

    if round(steps / walkAmount) == 11:
      walking = 0
      VIBRO.value(1)
      for j in range(3):
        for k in range(10):
          if j == 0:
            led.color(k, ((51,0,51)))
            led.on(k)
          if j == 1:
            led.color(k, ((255,255,0)))
            led.on(k)
          if j == 2:
            led.color(k, ((0,0,153)))
            led.on(k)
          utime.sleep_ms(100)
      VIBRO.value(0)

led.all_color(((204,51,204)))
led.all_on()

while True:
  accel.measure()
  z_axis=0
  z_axis=accel.get_float(zsensor.ACCEL_Z)
  utime.sleep(0.01)
  accel.measure()
  z_axis1=0
  z_axis1=accel.get_float(zsensor.ACCEL_Z)
  z_result = z_axis - z_axis1
  if z_result > 20 or z_result < -20:
    walk()
  led.all_color(((255,0,0)))
  led.all_on()
