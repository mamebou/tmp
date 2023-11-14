using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbedObjectTrace : MonoBehaviour
{
    public GameObject mqttManager;
    private mqttTrace mqtt;
    private bool isFollow = false;
    public GameObject hand;
    private Vector3 followPos = new Vector3();
    public GameObject fingerA;
    // Start is called before the first frame update
    void Start()
    {
        mqtt = mqttManager.GetComponent<mqttTrace>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mqtt.isOpenGripper == true)
            isFollow = false;
        if(isFollow == true){
            followPos = fingerA.transform.position;
            transform.position = followPos;
        }
    }

    void OnCollisionStay(Collision collision){
        if(collision.gameObject.tag == "hand" && mqtt.isOpenGripper == false){
            isFollow = true;
        }
    }
}
