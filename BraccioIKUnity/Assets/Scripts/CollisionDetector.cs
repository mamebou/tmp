using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// detect collision between robot and table for safty
public class CollisionDetector : MonoBehaviour
{
    public GameObject handTracker;
    private HandTrackerForUR handTrackerForUR;
    // Start is called before the first frame update
    void Start()
    {
        handTrackerForUR = handTracker.GetComponent<HandTrackerForUR>();
        
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "table"){
            handTrackerForUR.homePosition.transform.position = handTrackerForUR.indexTip.transform.position;
            handTrackerForUR.directionText.SetActive(true);
            handTrackerForUR.homePosition.SetActive(true);
            handTrackerForUR.isFinishCount = false;
        }
    }
}
