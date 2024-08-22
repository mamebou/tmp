import asyncio
from bleak import BleakClient

address = "FF:3C:75:38:36:2C"
UUID = "00002a24-0000-1000-8000-00805f9b34fb"

async def run(address, loop):
    async with BleakClient(address, loop=loop) as client:
        x = await client.is_connected()
        print("Connected: {0}".format(x))
        #y = await client.read_gatt_char(UUID)
        #print(y)

loop = asyncio.get_event_loop()
loop.run_until_complete(run(address, loop))