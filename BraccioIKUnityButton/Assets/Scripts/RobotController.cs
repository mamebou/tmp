using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RobotController : MonoBehaviour
{
    public GameObject rightButton;
    private bool isRightButtonPress = false;
    private bool isLeftButtonPress = false;
    private bool isUpButtonPress = false;
    private bool isDownButtonPress = false;
    private bool isFrontButtonPress = false;
    private bool isBackButtonPress = false;
    private bool isUpRotButtonPress = false;
    private bool isDownRotBuuttonPress = false;
    public GameObject IKControl;
    Vector3 position;
    public Vector3 rotation;
    public bool isGrab = false;
    public GameObject textArea;
    private TMPro.TMP_Text text;
    public GameObject mqtt;
    private MQTTForBUtton mqttForButton;
    //実験用
    public GameObject exController;
    private ExperimentManager exManager;
    private float adjuster = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        position = IKControl.transform.position;
        rotation = IKControl.transform.rotation.eulerAngles;
        text = textArea.GetComponent<TMPro.TMP_Text>();
        mqttForButton = mqtt.GetComponent<MQTTForBUtton>();
        //実験用
        exManager = exController.GetComponent<ExperimentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!exManager.finishExperiment)
            text.text = isGrab.ToString();
        if (isRightButtonPress)
        {
            position.x += adjuster;
            IKControl.transform.position = position;
        }

        if (isLeftButtonPress)
        {
            position.x -= adjuster;
            IKControl.transform.position = position;
        }

        if (isUpButtonPress)
        {
            position.y += adjuster;
            IKControl.transform.position = position;
        }

        if (isDownButtonPress)
        {
            position.y -= adjuster;
            IKControl.transform.position = position;
        }

        if (isFrontButtonPress)
        {
            position.z += adjuster;
            IKControl.transform.position = position;

        }

        if (isBackButtonPress)
        {
            position.z -= adjuster;
            IKControl.transform.position = position;
        }

        if(isUpRotButtonPress){
            if(rotation.z >= -90f)
                rotation.z += -0.1f;
        }

        if(isDownRotBuuttonPress){
            if(rotation.z <= 90f)
                rotation.z += 0.1f;
        }
    }

    public void RightButtonOnPress()
    {
        isRightButtonPress = true;
    }

    public void RightButtonReleese()
    {
        isRightButtonPress = false;
    }

    public void LeftButtonOnPress()
    {
        isLeftButtonPress = true;
    }

    public void LeftButtonReleese()
    {
        isLeftButtonPress = false;
    }

    public void UpButtonOnPress()
    {
        isUpButtonPress = true;
    }

    public void UpButtonReleese()
    {
        isUpButtonPress = false;
    }

    public void DownButtonOnPress()
    {
        isDownButtonPress = true;
    }

    public void DownButtonReleese()
    {
        isDownButtonPress = false;
    }

    public void FrontButtonOnPress()
    {
        isFrontButtonPress = true;
    }

    public void FrontButtonReleese()
    {
        isFrontButtonPress = false;
    }

    public void BackButtonOnPress()
    {
        isBackButtonPress = true;
    }

    public void BackButtonReleese()
    {
        isBackButtonPress = false;
    }

    public void UpRotButtonPress(){
        isUpRotButtonPress = true;
    }

    public void UpRotButtonReleese(){
        isUpRotButtonPress = false;
    }

    public void DownRotButtonPress(){
        isDownRotBuuttonPress = true;
    }

    public void DownRotButtonReleese(){
        isDownRotBuuttonPress = false;
    }

    public void grab(){
        isGrab = !isGrab;
        mqttForButton.isGrip = !mqttForButton.isGrip;
    }

}
