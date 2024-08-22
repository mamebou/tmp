
using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MQTTForBUtton : MonoBehaviour
{
    IMqttClient mqttClient;
    SolveIKButton IK;
    public GameObject aolver;
    private bool isConnect = false;
    private int count = 0;
    public float thetaWristVertical = 0f;
    public float thetaWristRotation = 90f;
    public GameObject handTracker;
	private HandTrackingTest tracker;
    private int numButton = 0;
    public bool isGrip = false;
    private int rcvCount = 0;
    public string hololensTopic = "mytopic/mqtt";
    public string testTopic = "mytopic/test";
    public bool wait = true;
    private float prevThetaWristVertical = 0f;
    private float secondPrevThetaWristVertical = 0f;
    private float currentThetaWristVertical = 0f;
    int gripCount = 5;
    bool changeGrip = true;    


    async void Start()
    {
        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();
        IK = aolver.GetComponent<SolveIKButton>();
        tracker = handTracker.GetComponent<HandTrackingTest>();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("broker.emqx.io")
            .Build();

        mqttClient.Connected += (s, e) => {
            Debug.Log("connected");
            isConnect = true;
        };

        mqttClient.Disconnected += async (s, e) =>
        {
            isConnect = false;
            Debug.Log("disconnected");

            if (e.Exception == null)
            {
                return;
            }

            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {
                await mqttClient.ConnectAsync(options);
            }
            catch
            {
                Debug.Log("faild to connect");
            }
        };

        mqttClient.ApplicationMessageReceived += (s, e) =>
        {
            string[] data = Encoding.UTF8.GetString(e.ApplicationMessage.Payload).Split(' ');
            int pressure = int.Parse(data[2]);
            if(pressure > 2000){
                gripCount--;
                if(gripCount < 0 && changeGrip){
                    isGrip = !isGrip;
                    gripCount = 5;
                    changeGrip = false;
                }
            }

            if(pressure < 200){
                gripCount--;
                if(gripCount < 0){
                    changeGrip = true;
                }
            }
                if(tracker.isFinishCount && tracker.isStart){
                    currentThetaWristVertical = roundOnePlace(float.Parse(data[1]));
                    if(prevThetaWristVertical == currentThetaWristVertical && currentThetaWristVertical == secondPrevThetaWristVertical){
                        thetaWristVertical = roundOnePlace(float.Parse(data[1]));
                    }
                    prevThetaWristVertical = currentThetaWristVertical;
                    secondPrevThetaWristVertical = prevThetaWristVertical;
                }

                rcvCount++;
                wait = false;
        };

        await mqttClient.ConnectAsync(options);

        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("emqx/esp32").Build());

    }

    async void Update(){
        count++;;
        if(count == 10){
            if(isGrip){
                IK.thetaGripper = 80f;
            }
            else{
                IK.thetaGripper = 0f;
            }

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("johnson65/helloworld")
                .WithPayload(Mathf.RoundToInt(IK.thetaBase) + "," + 
                            (280f - (Mathf.RoundToInt(IK.thetaShoulder) + 100f)) + "," + 
                            (180f - Mathf.RoundToInt(IK.thetaElbow)) + "," + 
                            (thetaWristVertical + 90f) + "," + 
                            Mathf.RoundToInt(thetaWristRotation) + "," + 
                            Mathf.RoundToInt(IK.thetaGripper) + ";")
                .WithExactlyOnceQoS()
                .Build();            
            

            if(IK.resetPublish){
                IK.resetPublish = false;
                await mqttClient.PublishAsync(message);
            }

            try{
                if(tracker.isFinishCount && tracker.isStart){
                    Debug.Log("publish");
                    await mqttClient.PublishAsync(message);
                }
                
            }
            catch{
  
            }
            count = 0;
        }
    }

    public float roundOnePlace(float num){
        if(num >= -90f && num < -44f){
            return -90f;
        }
        else if(num >= -44f && num < -18f) {
            return -45f;
        }
        else if(num >= -28f && num < 18f) {
            return 0f;
        }
        else if(num >= 18f && num < 54f) {
            return 45f;
        }
        else if(num >= 54f && num <= 90f) {
            return 90f;
        }
        else{
            return 0f;
        }

    }

    public float RoundToDecimalPlace(float number, int decimalPlaces)
    {
        float pow = Mathf.Pow(10,decimalPlaces);
        return Mathf.Round(number * pow) / pow;
    }

    async void OnDestroy(){
        await mqttClient.DisconnectAsync();
    }


}
