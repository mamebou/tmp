using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public GameObject receiveDetector;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(receiveDetector.GetComponent<PythonTest>().isRecieve){
            this.gameObject.transform.position = new Vector3(1f, 1f, 1f);
        }
    }
}
