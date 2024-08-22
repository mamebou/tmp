using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{
    public bool isCarry = false;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.GetComponent<BoxCollider>().enabled == false){
            count++;
            if(count == 60){
                this.gameObject.GetComponent<BoxCollider>().enabled = true;
                count = 0;
            }
        }
    }
}
