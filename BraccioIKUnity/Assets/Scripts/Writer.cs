using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class Writer : MonoBehaviour
{
    StreamWriter sw;
    public GameObject target;
    private Vector3 prevPos;
    private Vector3 prevRot;
    public GameObject mqttManager;
    private mqttTrace mqtt;
    private bool previsOpenGrip = true;
    private Vector3 dPosition;
    private Vector3 drotation;
    public bool isWrite = false;
    private bool prevIsWrite = false;
    // Start is called before the first frame update
    void Start()
    {
        mqtt = mqttManager.GetComponent<mqttTrace>();
        prevPos = target.transform.position;
        prevRot = target.transform.localEulerAngles;
        String file = Application.persistentDataPath + "/move.txt";

        if(!File.Exists(file)){
            Debug.Log("create file");
            sw = File.CreateText(file);
            sw.Flush();
            sw.Dispose();
            sw = new StreamWriter(file, true, Encoding.UTF8);
        }
        else{
            //overweite
            sw = new StreamWriter(file, false, Encoding.UTF8);
        }
        Debug.Log(file);
    }

    // Update is called once per frame
    void Update()
    {
        if(isWrite != prevIsWrite && isWrite == true){
            if(mqtt.isOpenGripper != previsOpenGrip){
                if(mqtt.isOpenGripper == false){
                    previsOpenGrip = false;
                    sw.WriteLine("gripper close");
                }
                else{
                    previsOpenGrip = true;
                    sw.WriteLine("gripper open");
                }
            }
            dPosition = target.transform.position - prevPos;
            drotation = target.transform.localEulerAngles - prevRot;
            sw.WriteLine("pose " + Convert.ToString(dPosition.x) + " " + Convert.ToString(dPosition.y) + " " + Convert.ToString(dPosition.z) + " " + Convert.ToString(drotation.x) + " " + Convert.ToString(drotation.y) + " " + Convert.ToString(drotation.z));
            sw.Flush();
            prevPos = target.transform.position;
            prevRot = target.transform.localEulerAngles;
        }

        if(isWrite != prevIsWrite && isWrite == false){
            sw.Flush();
            sw.Close();            
        }
    }
}
