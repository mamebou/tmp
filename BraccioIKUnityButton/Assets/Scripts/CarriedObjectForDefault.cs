using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarriedObjectForDefault : MonoBehaviour
{
    public GameObject robotController;
    private MQTTTest controller;
    private bool followHead = false;
    private bool beforeGrab;
    public GameObject robotHead;
    private EndController endController;

    // Start is called before the first frame update
    void Start()
    {
        controller = robotController.GetComponent<MQTTTest>();
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
            Debug.Log("Collision with head");
            if(controller.isGrip && !endController.isCarry){
                followHead = true;
                endController.isCarry = true;
            }
            if(!controller.isGrip && endController.isCarry){
                robotHead.GetComponent<BoxCollider>().enabled = false;
                endController.isCarry = false;
                followHead = false;
            }

        }
    }
}
