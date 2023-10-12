
using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using UnityEngine;

public class mqttForUR : MonoBehaviour
{
    IMqttClient mqttClient;
    public GameObject target;
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    async void Start()
    {
        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("test.mosquitto.org")
            .Build();

        mqttClient.Connected += (s, e) => Debug.Log("接続したときの処理");

        mqttClient.Disconnected += async (s, e) =>
        {
            Debug.Log("切断したときの処理");

            if (e.Exception == null)
            {
                Debug.Log("意図した切断です");
                return;
            }

            Debug.Log("意図しない切断です。５秒後に再接続を試みます");

            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {
                await mqttClient.ConnectAsync(options);
            }
            catch
            {
                Debug.Log("再接続に失敗しました");
            }
        };

        mqttClient.ApplicationMessageReceived += (s, e) =>
        {
            string[] data = Encoding.UTF8.GetString(e.ApplicationMessage.Payload).Split(' ');
            x = float.Parse(data[0]) * Mathf.Rad2Deg;
            y = float.Parse(data[1]) * Mathf.Rad2Deg;
            z = float.Parse(data[2]) * Mathf.Rad2Deg;
            Debug.Log(x);
        };

        await mqttClient.ConnectAsync(options);

        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("mytopic/mqtt").Build());

        var message = new MqttApplicationMessageBuilder()
            .WithTopic("mytopic/ur")
            .WithPayload("Hello World")
            .WithExactlyOnceQoS()
            .Build();

        await mqttClient.PublishAsync(message);
    }

    async void OnDestroy(){
        await mqttClient.DisconnectAsync();
    }
}
