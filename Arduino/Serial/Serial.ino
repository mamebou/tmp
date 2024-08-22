#include<Braccio.h>
#include<Servo.h>

Servo base;
Servo shoulder;
Servo elbow;
Servo wrist_ver;
Servo wrist_rot;
Servo gripper;

String data = "hello";
String cmds[5] = {"\0"};
int index = 5; 
void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  Braccio.begin();
}

void loop() {
  // put your main code here, to run repeatedly:
  if (Serial.available() > 0) {
    data = Serial.readStringUntil(';');
    Serial.println(data);
    Braccio.ServoMovement(20, 
      getValue(data, ',', 0).toInt(), 
      getValue(data, ',', 1).toInt(), 
      getValue(data, ',', 2).toInt(), 
      getValue(data, ',', 3).toInt(), 
      getValue(data, ',', 4).toInt(), 
      getValue(data, ',', 5).toInt());
  }
}

String getValue(String data, char separator, int index)
{
  int found = 0;
  int strIndex[] = {0, -1};
  int maxIndex = data.length()-1;

  for(int i=0; i<=maxIndex && found<=index; i++){
    if(data.charAt(i)==separator || i==maxIndex){
        found++;
        strIndex[0] = strIndex[1]+1;
        strIndex[1] = (i == maxIndex) ? i+1 : i;
    }
  }

  return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}
