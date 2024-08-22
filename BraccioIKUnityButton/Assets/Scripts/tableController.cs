using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableController : MonoBehaviour
{
    public GameObject handTracker;
    private HandTrackingTest tracker;
    // Start is called before the first frame update
    void Start()
    {
        tracker = handTracker.GetComponent<HandTrackingTest>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.tag == "hand"){
            tracker.menue();
        }
    }
}
