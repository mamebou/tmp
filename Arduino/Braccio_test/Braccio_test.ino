#include<Braccio.h>
#include<Servo.h>

Servo base;
Servo shoulder;
Servo elbow;
Servo wrist_ver;
Servo wrist_rot;
Servo gripper;

void setup() {
  // put your setup code here, to run once:
  Braccio.begin();
  Braccio.ServoMovement(20, 10, 10, 110, 90, 50, 53);
}

void loop() {
}
