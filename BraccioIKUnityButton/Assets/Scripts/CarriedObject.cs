using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CarriedObject : MonoBehaviour
{
    public GameObject robotController;
    private RobotController controller;
    private bool followHead = false;
    private bool beforeGrab;
    public GameObject robotHead;
    private EndController endController;
    // Start is called before the first frame update
    void Start()
    {
        controller = robotController.GetComponent<RobotController>();
        endController = robotHead.GetComponent<EndController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(followHead){
            this.gameObject.transform.position = robotHead.transform.position;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "hand"){
            Debug.Log("collision with hand");
            if(controller.isGrab && !endController.isCarry){
                followHead = true;
                endController.isCarry = true;
            }
            if(!controller.isGrab && endController.isCarry){
                robotHead.GetComponent<BoxCollider>().enabled = false;
                endController.isCarry = false;
                followHead = false;
            }
            
        }
    }
}
