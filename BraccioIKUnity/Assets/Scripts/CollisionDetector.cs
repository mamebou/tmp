using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        handTrackerForUR.homePosition.transform.position = handTrackerForUR.indexTip.transform.position;
        handTrackerForUR.directionText.SetActive(true);
        handTrackerForUR.homePosition.SetActive(true);
        handTrackerForUR.isFinishCount = false;
        Debug.Log("hello");
    }
}
