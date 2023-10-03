from zbtm import Mesh
import time
from zled import Led
import random
m = Mesh("d")
m.ttl(5000)
l = Led("AI2AI_LED")
l.all_on()

col = random.randint(0, 2)
if (col == 0):
    l.all_color([255,0,0])
    str = "r"
elif (col == 1):
    l.all_color([0, 255, 0])
    str = "g"
else:
    l.all_color([0, 0, 255])
    str = "b"

while (True):
    wait = random.randint(1000, 1500)
    time.sleep_ms(wait)
    rc = m.near_with_message("r")
    gc = m.near_with_message("g")
    bc = m.near_with_message("b")
    if ((rc < gc) and (rc < bc)):
        l.all_color([255, 0, 0])
        str = "r"
    elif ((gc < rc) and (gc < bc)):
        l.all_color([0, 255, 0])
        str = "g"
    elif ((bc < gc) and (bc < rc)):
        l.all_color([0, 0, 255])
        str = "b"
    elif (rc < gc):
        str = "r"
        l.all_color([255, 0, 0])
    elif (gc < bc):
        l.all_color([0, 255, 0])
        str = "g"
    elif (bc < rc):
        l.all_color([0, 0, 255])
        str = "b"
    m.send(str)
    m.receive()
    print(rc)
    print(gc)
    print(bc)

