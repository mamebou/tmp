#include <SPI.h>
#include <UIPEthernet.h>
#include <PubSubClient.h>

EthernetClient ethClient;
PubSubClient mqttClient(ethClient);

// Function prototypes
void subscribeReceive(char* topic, byte* payload, unsigned int length);

byte mac[] = { 0x90, 0xA2, 0xDA, 0x0D, 0x78, 0xEE  };  
IPAddress ip(192, 168, 0, 117);
IPAddress myDns(192, 168, 0, 1);

// Make sure to leave out the http and slashes!
const char* server = "192.168.0.104";

void setup()
{

  // Useful for debugging purposes
  Serial.begin(9600);

  // Start the ethernet connection
  Ethernet.begin(mac, ip ,myDns);              

  // Ethernet takes some time to boot!
  delay(3000);                          

  // Set the MQTT server to the server stated above ^
  mqttClient.setServer(server, 1883);   

  if (mqttClient.connect("myArduinoID")) {

    Serial.println("Connection has been established, well done");

    // Establish the subscribe event
    mqttClient.setCallback(subscribeReceive);

  } else {

    Serial.println("Looks like the server connection failed...");

  }

}

void loop(){

  mqttClient.loop();

  mqttClient.subscribe("test");

  if(mqttClient.publish("test", "Hello World from arduino")){

    Serial.println("Publish message success");

  }else{

    Serial.println("Could not send message :(");

  }

  // Dont overload the server!
  delay(4000);

}

void subscribeReceive(char* topic, byte* payload, unsigned int length){

  Serial.print("Topic: ");
  Serial.println(topic);

  Serial.print("Message: ");
  for(int i = 0; i < length; i ++){
    Serial.print(char(payload[i]));
  }
  Serial.println("");

}
