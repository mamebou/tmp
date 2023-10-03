using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandTrackingTest : MonoBehaviour
{
    //表示するオブジェクトを定義
    GameObject indexTip;
    public GameObject target;
    private float[] dPosition = new float[3] {0f, 0f, 0f};
    private float[] drotation = new float[3] {0f, 0f, 0f};
    //hard coded 
    private int jointNum = 6;
    private float[] pPos = new float[3];
    private float[] pRot = new float[3];
    public float adjust = 5f;
    private bool isFirstDitect = true;

    private const float PinchThreshold = 0.7f;
    private const float GrabThreshold = 0.4f;
    public bool isGrab = false;
    public bool isStart = false;


    void Start()
    {
        indexTip = GameObject.CreatePrimitive(PrimitiveType.Cube);  //表示用の立方体を作成
        indexTip.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);  //大きさを指定
    }

    void Update()
    {
        //右手の人差し指の指先の位置情報を取得
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, Handedness.Right, out MixedRealityPose pose))
        {
            indexTip.transform.position = pose.Position; //座標を設定
            indexTip.transform.rotation = pose.Rotation; //回転を設定 default pose.rotation

            if(isStart){
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

                Quaternion rot = transform.rotation;
                rot.x = target.transform.rotation.x + drotation[0];
                rot.y = target.transform.rotation.y + drotation[1];
                rot.z = target.transform.rotation.z + drotation[2];

                //position更新
                target.transform.position = pos;

                //姿勢更新
                target.transform.rotation = rot;
                //ひとつ前の位置姿勢更新

                pPos[0] = indexTip.transform.position.x;
                pPos[1] = indexTip.transform.position.y;
                pPos[2] = indexTip.transform.position.z;
                pRot[0] = indexTip.transform.rotation.x;
                pRot[1] = indexTip.transform.rotation.y;
                pRot[2] = indexTip.transform.rotation.z;
            }
            
        }
    }

    public static bool IsPinching(Handedness trackedHand)
    {
        return HandPoseUtils.CalculateIndexPinch(trackedHand) > PinchThreshold;
    }

    public static bool IsGrabbing(Handedness trackedHand)
    {
        return !IsPinching(trackedHand) &&
                HandPoseUtils.MiddleFingerCurl(trackedHand) > GrabThreshold &&
                HandPoseUtils.RingFingerCurl(trackedHand) > GrabThreshold &&
                HandPoseUtils.PinkyFingerCurl(trackedHand) > GrabThreshold &&
                HandPoseUtils.ThumbFingerCurl(trackedHand) > GrabThreshold;
    }

    public void start(){
        isStart = true;
    }

    public void reset(){
        isFirstDitect = true;
    }


}