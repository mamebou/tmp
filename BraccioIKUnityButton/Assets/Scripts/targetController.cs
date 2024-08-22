using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetController : MonoBehaviour
{
    public GameObject target;
    public Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        rotation = target.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rotationXPositive(){
        rotation.x += 5f;
        target.transform.rotation = rotation;
    }

    public void rotationYPositive(){

    }

    public void rotationZPositive(){

    }


}
