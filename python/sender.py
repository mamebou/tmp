import os
os.environ['PYTHONASYNCIODEBUG'] = '1'
import asyncio
import logging
import cv2
import base64
import cv2
import numpy as np
import numpy as np
import scipy
import scipy.misc
import matplotlib.pyplot as plt
import VideoGradientTest
from PIL import Image

logging.basicConfig(level=logging.ERROR)
async def tcp_echo_client(data, loop):
    reader, writer = await asyncio.open_connection('192.168.86.51', 8080, loop=loop)
    print('Sending data of size: %r' % str(len(data)))

    #sending the size and data byte array
    print("Sending data: " + str(len(data)) + str(data))
    writer.write(str(len(data)).encode() + str(data).encode())
    await writer.drain()
    #print("Message: %r" %(data))
    print(len(data))
    print('Close the socket')
    writer.write_eof()
    #writer.close()





def grab_frame(cap):
    ret, frame = cap.read()
    width = cap.get(3)  # float
    height = cap.get(4)  # float
    gMagImg = VideoGradientTest.getWindowedGradientMagnitude(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB), int(width), int(height))

    FrameTempTest = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    return gMagImg[0:650, 0:1200]

# START HERE
#  Initiate the camera
cap = cv2.VideoCapture(0)
cap.set(3, 1280)
cap.set(4, 720)
width = cap.get(3)
height = cap.get(4)
print(width)
print(height)


#  create a subplot
fig = plt.figure(frameon=False)
plt.axis("off")
fig.set_size_inches(4 ,3)

#make the content fit the whole figure
ax = plt.Axes(fig, [0., 0., 1., 1.])
ax.set_axis_off()
fig.add_axes(ax)
ax.axes.get_xaxis().set_visible(False)
ax.axes.get_yaxis().set_visible(False)
#continually send frames
while cv2.waitKey(1) & 0xFF != ord('q'):
    frame = grab_frame(cap)
    #draw your image to the plot

    #imPlot.set_data(frame) #PLOT THE BITS DATA
    plt.contourf(frame) #Take the contour of the imgData
    plt.savefig('foo.jpg', bbox_inches='tight', transparent=True, pad_inches=0)
    with open("foo.jpg", "rb") as imageFile:
        data = base64.b64encode((imageFile.read()))


    loop = asyncio.get_event_loop()
    loop.run_until_complete(tcp_echo_client(data, loop))

plt.ioff()
loop.close()