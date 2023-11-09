using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using TMPro;
using UnityEngine.Events;

public class handTrackerTrace : MonoBehaviour
{
    private GameObject indexTip;
    public GameObject target;
    private float[] dPosition = new float[3] {0f, 0f, 0f};
    private float[] drotation = new float[3] {0f, 0f, 0f};
    private bool isFirstDitect = true;
    private float[] pPos = new float[3];
    private float[] pRot = new float[3];
    public float adjust = 1f;
    private mqttTrace mqtt;
    public GameObject mqttManager;
    float theta = 0f;
    private float thetaAdjust;
    private float inverse = 1f;
    private float thetaX = 0f;
    private float thetaY = 0f;
    public bool isStart = false;
    public bool isFinishCount = false;
    public bool resetRobot = false;
    private Vector3 initialTargetPosition;
    private Vector3 initialHomePosition;
    public float countDown = 3f;
    private float prevX = 0f;
    private float prevY = 180f;
    public bool moveActualRobot = false;
    public GameObject directionText;
    public GameObject homePosition;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject textComponent;
    public bool isControlDirection = false;
    public bool isBeforeMoveRobot = false;
    public bool isFinishMove = true;
    public GameObject fingerA;
    public GameObject fingerB;
    private bool previsOpenGrip = true;
    public GameObject writerManager;
    private Writer writer;

    // Start is called before the first frame update
    void Start()
    {
        indexTip = GameObject.CreatePrimitive(PrimitiveType.Cube);  //表示用の立方体を作
        indexTip.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);  //大きさを指定
        initialTargetPosition = target.transform.position;
        initialHomePosition = homePosition.transform.position;
        mqtt = mqttManager.GetComponent<mqttTrace>();
        writer = writerManager.GetComponent<Writer>();
        yesButton.SetActive(false);
        noButton.SetActive(false);
        directionText.SetActive(false);
        homePosition.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isControlDirection == true){
            beforeStart();
            isControlDirection = false;
        }

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out MixedRealityPose pose) && isStart){
            indexTip.transform.position = pose.Position; //座標を設定
            indexTip.transform.rotation = pose.Rotation; //回転を設定 default pose.rotation
            if(isFinishCount){
                homePosition.transform.position = pose.Position;
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

                Vector3 pos = target.transform.position;
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

                Vector3 rot = target.transform.localEulerAngles;

                if((mqtt.y + 180f) < 140f){
                    rot.y = 110f;
                }
                else if((mqtt.y + 180f) > 200f){
                    rot.y = 260f;
                }
                else{
                    rot.y = 180f;
                }
                if(mqtt.x < -50f){
                    rot.x = -90f;
                }
                else if(mqtt.x > 40f){
                    rot.x = 90f;
                }
                else{
                    rot.x = 0f;
                }
                rot.x = mqtt.x;
                rot.y = mqtt.y;
                rot.z = mqtt.z;

                //target.transform.localEulerAngles = rot;

                if(mqtt.isOpenGripper != previsOpenGrip){
                    if(mqtt.isOpenGripper == false){
                        previsOpenGrip = false;
                        fingerA.transform.localPosition = mqtt.fingerAClose;
                        fingerB.transform.localPosition = mqtt.fingerBClose;
                    }
                    else{
                        previsOpenGrip = true;
                        fingerA.transform.localPosition = mqtt.initialFingerA;
                        fingerB.transform.localPosition = mqtt.initialFingerB;
                    }
                }


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
                        writer.isWrite = true;
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

        if(isBeforeMoveRobot == true && isStart == false)
            beforeMoveRobot(); 
        
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

    public void moveRobot(){
        yesButton.SetActive(false);
        noButton.SetActive(false);
        directionText.SetActive(false);
        isFinishMove = false;
        moveActualRobot = true;
        isBeforeMoveRobot = false;
        homePosition.transform.position = initialHomePosition;
        target.transform.position = initialTargetPosition;
    }

    public void beforeStart(){
        textComponent.GetComponent<TMP_Text>().text = "put your hand on the orenge cube";
        directionText.SetActive(true);
        homePosition.SetActive(true);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        isStart = true;
        isFinishCount = false;
    }

    public void beforeMoveRobot(){
        textComponent.GetComponent<TMP_Text>().text = "Are you sure to move the robot?";
        homePosition.SetActive(false);
        directionText.SetActive(true);
        yesButton.SetActive(true);
        noButton.SetActive(true);
        writer.isWrite = false;
    }

    public void No(){
        directionText.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        isStart = false;
        isControlDirection = false;
        isBeforeMoveRobot = false;
        textComponent.GetComponent<TMP_Text>().text = "push control button on the ball";
        directionText.SetActive(true);
    }

    public void finishMove(){
        textComponent.GetComponent<TMP_Text>().text = "push control button on the ball";
        directionText.SetActive(true);
        yesButton.SetActive(false);
        noButton.SetActive(false);
        homePosition.SetActive(false);
        isStart = false;
        isBeforeMoveRobot = false;
    }
}
