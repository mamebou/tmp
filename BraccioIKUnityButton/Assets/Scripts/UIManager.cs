using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject IKsolver;
    public GameObject startButton;
    public GameObject resetButton;
    private SolveIK IK;
    private bool isStart = false;
    public bool resetRobot = false;

    // Start is called before the first frame update
    void Start()
    {
        IK = IKsolver.GetComponent<SolveIK>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //reset target position
    public void reset(){
        isStart = false;
        resetRobot = true;
        IK.targetPosition = IK.initialTargetPosition;
        IKsolver.transform.position = IK.initialTargetPosition;
    }

    //to start controll
    public void start(){
        startButton.SetActive(false);
        resetButton.SetActive(false);
        isStart = true;
    }
}
