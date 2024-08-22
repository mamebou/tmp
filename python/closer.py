import serial

#シリアル通信(PC⇔Arduino)
ser = serial.Serial()
ser.port = "COM8"     #デバイスマネージャでArduinoのポート確認
ser.baudrate = 115200 #Arduinoと合わせる
ser.setDTR(False)     #DTRを常にLOWにしReset阻止
ser.close()           #COMポートを閉じる
