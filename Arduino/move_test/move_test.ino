#include<Braccio.h>
#include<Servo.h>

Servo base;
Servo shoulder;
Servo elbow;
Servo wrist_ver;
Servo wrist_rot;
Servo gripper;
void setup() {
  unsigned long time;
  // put your setup code here, to run once:
  Serial.begin(115200);
  Braccio.begin();
  time = millis();
  Serial.println(time);
  Braccio.ServoMovement(90, 90, 60, 90, 90, 90, 50);
}

void loop() {
  // put your main code here, to run repeatedly:

}
