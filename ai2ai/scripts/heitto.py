import time
import math
import zled
import zsensor
import utime
from zsensor import Sensor
from machine import Pin

led = zled.Led()



# Accelerometer
accel = Sensor("accelerometer")
VIBRO = Pin(("GPIO_EXP1", 2 or 3), Pin.OUT)
VIBRO.off()

# Read data
y=[];
time_points=[];
for k in range(60):
   if ( (k==0) | (k==59)): led.all_color(((0,102,0))); led.all_on(); VIBRO.value(1); time.sleep(0.3); led.all_off(); VIBRO.value(0);
   time_points.append(utime.ticks_ms()); accel.measure(); y.append(round(math.sqrt(accel.get_float(zsensor.ACCEL_X)**2+accel.get_float(zsensor.ACCEL_Y)**2+accel.get_float(zsensor.ACCEL_Z)**2),3)); time.sleep(0.05);



# Find minimum location
tmp_min=10000;
for k in range(len(y)):
   if y[k]<tmp_min: tmp_min=y[k];



k_min=0;
for k in range(len(y)):
   if y[k]==tmp_min: k_min=k;



# Find beginning of the jump
jump_start=0;
k=k_min;
cont=1;
while cont:
   k=k-1;
   if k<-1: cont=0;
   elif ( (y[k+1]>15) & (y[k+1]>y[k+2]) ): jump_start=k+1; cont=0;



# Find end of the jump
jump_end=len(y)-1;
k=k_min;
cont=1;
while cont:
   k=k+1;
   if k>=len(y): cont=0;
   elif ( (y[k-1]>15) & (y[k-1]>y[k]) ): jump_end=k; cont=0;

# Define jump height
jump_time= time_points[jump_end]-time_points[jump_start];
jump_height=(9.81/8.)*(jump_time/1000)**2;

print(round(jump_height*100));
print(jump_time);

led.all_color([0,255,0]);
if jump_height>=0.05: led.on(0);

if jump_height>=0.10: led.on(1);

if jump_height>=0.15: led.on(2);

if jump_height>=0.20: led.on(3);

if jump_height>=0.25: led.on(4);

if jump_height>=0.30: led.on(5);

if jump_height>=0.35: led.on(6);

if jump_height>=0.40: led.on(7);

if jump_height>=0.45: led.on(8);

if jump_height>=0.50: led.on(9);

if jump_height>=0.55: led.color(0,((255,0,0)));

if jump_height>=0.60: led.color(1,((255,0,0)));

if jump_height>=0.65: led.color(2,((255,0,0)));

if jump_height>=0.70: led.color(3,((255,0,0)));

if jump_height>=0.75: led.color(4,((255,0,0)));

if jump_height>=0.80: led.color(5,((255,0,0)));

if jump_height>=0.85: led.color(6,((255,0,0)));

if jump_height>=0.90: led.color(7,((255,0,0)));

if jump_height>=0.95: led.color(8,((255,0,0)));

if jump_height>=1.00: led.color(9,((255,0,0)));

time.sleep(5);
led.all_off();