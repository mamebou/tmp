using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public GameObject rightButton;
    private bool isRightButtonPress = false;
    private bool isLeftButtonPress = false;
    private bool isUpButtonPress = false;
    private bool isDownButtonPress = false;
    private bool isFrontButtonPress = false;
    private bool isBackButtonPress = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRightButtonPress)
        {
            Debug.Log("hello");
        }

        if (isLeftButtonPress)
        {
            Debug.Log("left");
        }

        if (isUpButtonPress)
        {
            Debug.Log("up");
        }

        if (isDownButtonPress)
        {
            Debug.Log("down");
        }

        if (isFrontButtonPress)
        {
            Debug.Log("fron");

        }

        if (isBackButtonPress)
        {
            Debug.Log("back");
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
}
