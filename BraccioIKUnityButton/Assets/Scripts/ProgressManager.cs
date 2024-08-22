using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class ProgressManager : MonoBehaviour
{
    public GameObject target;
    private bool isMove = true;
    private Vector3 pos;
    private Vector3 rot;
    public GameObject fingerA;
    public GameObject fingerB;
    private Vector3 initialFingerA;
    private Vector3 initialFingerB;
    private Vector3 fingerAClose;
    private Vector3 fingerBClose;
    String path;
    private int count = 0;
    private int size = 0;
    private float adjustHeight = 0f;
    List<List<String>> dir = new List<List<String>> ();
    public GameObject table;
    // Start is called before the first frame update
    void Start()
    {
        path = Application.persistentDataPath + "/move.txt";
        initialFingerA = fingerA.transform.localPosition;
        initialFingerB = fingerB.transform.localPosition;
        Vector3 tmp = fingerA.transform.localPosition;
        tmp.z = 0.1f;
        fingerAClose = tmp;
        tmp = fingerB.transform.localPosition;
        tmp.z = -0.1f;
        fingerBClose = tmp;
        ReadFile(path);
        size = dir.Count;
        Debug.Log(size);
    }

    // Update is called once per frame
    void Update()
    {
        if(count < size){
            moveRobot(dir[count]);
            count++;
        }
    }

    //read move direction file and store it in list
    private void ReadFile(String path){
        using (var fs = new StreamReader(path, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            while (fs.Peek() != -1)
            {
                String[] data = fs.ReadLine().Split(' ');
                List<String> line = new List<String>();
                for(int i = 0; i < data.Length; i++){
                    line.Add(data[i]);
                }
                dir.Add(line);
            }
        }
    }

    public void moveRobot(List<String> data){
        if(data[0] == "pose"){
            pos.x = target.transform.position.x + float.Parse(data[1]);
            pos.y = target.transform.position.y + float.Parse(data[2]);
            pos.z = target.transform.position.z + float.Parse(data[3]);
            rot.x = target.transform.localEulerAngles.x + float.Parse(data[4]);
            rot.y = target.transform.localEulerAngles.y + float.Parse(data[5]);
            rot.z = target.transform.localEulerAngles.z + float.Parse(data[6]);
            target.transform.position = pos;
            target.transform.localEulerAngles = rot;
        }
        else if(data[0] == "gripper"){
            if(data[1] == "open"){
                fingerA.transform.localPosition = initialFingerA;
                fingerB.transform.localPosition = initialFingerB;
            }
            else{
                fingerA.transform.localPosition = fingerAClose;
                fingerB.transform.localPosition = fingerBClose;
            }
        }
        else if(data[0] == "initialPose"){
            pos.x = float.Parse(data[1]) + table.transform.position.x;
            pos.y = float.Parse(data[2]) + table.transform.position.y;
            pos.z = float.Parse(data[3]) + table.transform.position.z;
            rot.x = float.Parse(data[4]);
            rot.y = float.Parse(data[5]);
            rot.z = float.Parse(data[6]);
            target.transform.position = pos;
            target.transform.localEulerAngles = rot;            
        }
    }
}
