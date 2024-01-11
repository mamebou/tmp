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
    public GameObject IKControl;
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        position = IKControl.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRightButtonPress)
        {
            position.x += 0.0001f;
            IKControl.transform.position = position;
        }

        if (isLeftButtonPress)
        {
            position.x -= 0.0001f;
            IKControl.transform.position = position;
        }

        if (isUpButtonPress)
        {
            position.y += 0.0001f;
            IKControl.transform.position = position;
        }

        if (isDownButtonPress)
        {
            position.y -= 0.0001f;
            IKControl.transform.position = position;
        }

        if (isFrontButtonPress)
        {
            position.z += 0.0001f;
            IKControl.transform.position = position;

        }

        if (isBackButtonPress)
        {
            position.z -= 0.0001f;
            IKControl.transform.position = position;
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
