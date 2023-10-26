using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class HandTrackerForUR : MonoBehaviour
{
    public GameObject indexTip;
    public GameObject target;
    private float[] dPosition = new float[3] {0f, 0f, 0f};
    private float[] drotation = new float[3] {0f, 0f, 0f};
    private bool isFirstDitect = true;
    private float[] pPos = new float[3];
    private float[] pRot = new float[3];
    public float adjust = 1f;
    public GameObject mqttManager;
    private mqttForUR mqtt;
    float theta = 0f;
    public GameObject baseObject;
    private float thetaAdjust;
    private float inverse = 1f;
    private float thetaX = 0f;
    private float thetaY = 0f;
    public GameObject Wrist1;
    public GameObject Wrist2;
    public GameObject startButton;
    public GameObject resetButton;
    public GameObject directionText;
    public GameObject homePosition;
    public bool isStart = false;
    public bool isFinishCount = false;
    public bool resetRobot = false;
    private Vector3 initialTargetPosition;
    public float countDown = 3f;
    private float prevX = 0f;
    private float prevY = 180f;
    private Vector3 initisalHomePosition;

    // Start is called before the first frame update
    void Start()
    {
        indexTip = GameObject.CreatePrimitive(PrimitiveType.Cube);  //表示用の立方体を作成
        indexTip.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);  //大きさを指定
        mqtt = mqttManager.GetComponent<mqttForUR>();
        initialTargetPosition = target.transform.position;
        initisalHomePosition = homePosition.transform.position;
        directionText.SetActive(false);
        homePosition.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out MixedRealityPose pose)){
            indexTip.transform.position = pose.Position; //座標を設定
            indexTip.transform.rotation = pose.Rotation; //回転を設定 default pose.rotation

            if(isStart){
                if(isFinishCount){
                    if(isFirstDitect == true){
                        dPosition[0] = 0f;
                        dPosition[1] = 0f;
                        dPosition[2] = 0f;
                        drotation[0] = 0f;
                        drotation[1] = 0f;
                        drotation[2] = 0f;
                        isFirstDitect = false;
                    }else{
                        //変化量
                        dPosition[0] = indexTip.transform.position.x - pPos[0];
                        dPosition[1] = indexTip.transform.position.y - pPos[1];
                        dPosition[2] = indexTip.transform.position.z - pPos[2];
                        drotation[0] = indexTip.transform.rotation.x - pRot[0];
                        drotation[1] = indexTip.transform.rotation.y - pRot[1];
                        drotation[2] = indexTip.transform.rotation.z - pRot[2];
                    }

                    Vector3 pos = transform.position;
                    pos.x = target.transform.position.x + dPosition[0] * adjust;
                    pos.y = target.transform.position.y + dPosition[1] * adjust;
                    pos.z = target.transform.position.z + dPosition[2] * adjust;

                    //Quaternion rot = new Quaternion();
                    //rot.x = mqtt.x;
                    //rot.y = mqtt.z;
                    //rot.z = mqtt.y;
                    //rot.w = mqtt.w;
                    

                    //position更新
                    target.transform.position = pos;
                    //
                    Vector3 rot = target.transform.localEulerAngles;
                    if(Mathf.Abs(prevX - mqtt.y) > 27f && Mathf.Abs(prevY - (mqtt.x + 180)) < 10f){
                        rot.x = mqtt.y;
                        prevX = rot.x;    
                    }

                    if(Mathf.Abs(prevY - (mqtt.x + 180)) > 10f && Mathf.Abs(prevX - mqtt.y) < 10f){
                        rot.y = mqtt.x + 180f;
                        rot.x = 0f;
                        prevY = rot.y;    
                    }
                    rot.y = mqtt.x + 180f;
                    rot.x = 0f;
   
                    target.transform.localEulerAngles = rot;


                    //ひとつ前の位置姿勢更新

                    pPos[0] = indexTip.transform.position.x;
                    pPos[1] = indexTip.transform.position.y;
                    pPos[2] = indexTip.transform.position.z;
                    pRot[0] = indexTip.transform.rotation.x;
                    pRot[1] = indexTip.transform.rotation.y;
                    pRot[2] = indexTip.transform.rotation.z;
                }
                else{
                    if(judgeDistance(pose)){
                        countDown -= Time.deltaTime;
                        if(countDown < 0f){
                            isFinishCount = true;
                            homePosition.SetActive(false);
                            directionText.SetActive(false);
                            countDown = 3f;
                        }
                    }
                    else{
                        countDown = 3f;
                    }
                }
            }
        }
        
    }

    public void start(){
        startButton.SetActive(false);
        resetButton.SetActive(false);
        if(!isFinishCount){
            directionText.SetActive(true);
            homePosition.SetActive(true);
        }
        isStart = true;
    }

    public void reset(){
        isStart = false;
        isFirstDitect = true;
        isFinishCount = false;
        resetRobot = true;
        target.transform.position = initialTargetPosition;
        homePosition.transform.position = initisalHomePosition;
    }

    public void menue(){
        if(startButton.activeSelf){
            isStart = true;
            startButton.SetActive(false);
            resetButton.SetActive(false);
        }
        else{
            isStart = false;
            startButton.SetActive(true);
            resetButton.SetActive(true);
        }
    }

    public bool judgeDistance(MixedRealityPose pose){
        double x = (double)pose.Position.x - (double)homePosition.transform.position.x;
        double y = (double)pose.Position.y - (double)homePosition.transform.position.y;
        double z = (double)pose.Position.z - (double)homePosition.transform.position.z;
        double distance = Math.Sqrt(x * x) + Math.Sqrt(y * y) + Math.Sqrt(z * z);
        if(distance < 0.1){
            return true;
        }
        else{
            return false;
        }
    }
}
