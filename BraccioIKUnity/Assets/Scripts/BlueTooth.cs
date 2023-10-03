using UnityEngine; 
using System.Collections;
using System.IO.Ports;



public class BlueTooth : MonoBehaviour
{
    private SerialPort serialPort;
    private string portName = "COM1"; // シリアルポートの名前を適切に設定

    void Start()
    {
        serialPort = new SerialPort(portName, 9600); // ボーレートを適切に設定
        serialPort.Open();
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            string data = serialPort.ReadLine();
            Debug.Log("Received: " + data);
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

}
