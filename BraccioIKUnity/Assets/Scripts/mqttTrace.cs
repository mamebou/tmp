
using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using UnityEngine;
using TMPro;

public class mqttTrace : MonoBehaviour
{
    IMqttClient mqttClient;
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float w = 0f;
    private int count = 0;
    private bool isConnect = false;
    public int isControl = 0;
    public GameObject handTracker;
    private handTrackerTrace tracker;
    public GameObject fingerA;
    public GameObject fingerB;
    public Vector3 initialFingerA;
    public Vector3 initialFingerB;
    public Vector3 fingerAClose;
    public Vector3 fingerBClose;
    public bool isOpenGripper = true;

    async void Start()
    {
        tracker = handTracker.GetComponent<handTrackerTrace>();
        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();
        initialFingerA = fingerA.transform.localPosition;
        initialFingerB = fingerB.transform.localPosition;
        Vector3 tmp = fingerA.transform.localPosition;
        tmp.z = 0.1f;
        fingerAClose = tmp;
        tmp = fingerB.transform.localPosition;
        tmp.z = -0.1f;
        fingerBClose = tmp;
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("test.mosquitto.org")
            .Build();

        mqttClient.Connected += (s, e) =>{
            Debug.Log("connected");
            isConnect = true;
        }; 
        mqttClient.Disconnected += async (s, e) =>
        {
            Debug.Log("disconnected");
            isConnect = false;
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
            if(data[0] == "control"){
                if(tracker.isStart == true){
                    tracker.isStart = false;
                    tracker.isControlDirection = false;
                    tracker.isBeforeMoveRobot = true;
                }
                else{
                    tracker.isStart = true;
                    tracker.isControlDirection = true;
                    tracker.isBeforeMoveRobot = false;
                }
            }
            else if(data[0] == "gripper"){
                if(isOpenGripper == true){
                    isOpenGripper = false;
                }
                else{
                    isOpenGripper = true;
                }                
            }
            else{
                x = float.Parse(data[0]) * Mathf.Rad2Deg;
                y = float.Parse(data[1]) * Mathf.Rad2Deg;
                z = float.Parse(data[2]) * Mathf.Rad2Deg;
                isControl = int.Parse(data[4]);
            }

        };

        await mqttClient.ConnectAsync(options);

        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("mytopic/mqtt").Build());

        var message = new MqttApplicationMessageBuilder()
            .WithTopic("harutakatopic/ur")
            .WithPayload("Hello World")
            .WithExactlyOnceQoS()
            .Build();

        await mqttClient.PublishAsync(message);
    }

    async void Update(){
        count++;
        if(count == 10 && isConnect){
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("harutakatopic/ur")
                .WithPayload("\"robopose\":p[]")
                .WithExactlyOnceQoS()
                .Build();
            await mqttClient.PublishAsync(message);
            count = 0;
        }
    }

    async void OnDestroy(){
        await mqttClient.DisconnectAsync();
    }
}
