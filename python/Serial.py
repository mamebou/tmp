import serial

#シリアル通信(PC⇔Arduino)
ser = serial.Serial()
ser.port = "COM8"     #デバイスマネージャでArduinoのポート確認
ser.baudrate = 115200 #Arduinoと合わせる
ser.setDTR(False)     #DTRを常にLOWにしReset阻止
ser.open()            #COMポートを開く
ser.write(b'hello world;')       #送りたい内容をバイト列で送信
ser.close()           #COMポートを閉じる
