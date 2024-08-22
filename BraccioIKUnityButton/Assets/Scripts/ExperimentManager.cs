using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ExperimentManager : MonoBehaviour
{
    public float[] accuracyResult = new float[3] {0.0f, 0.0f, 0.0f}; 
    public int index = 0;
    public DateTime startTime;
    public DateTime endTime; 
    public bool startExperiment = false;
    public bool finishExperiment = false;
    public GameObject textArea;
    private TMPro.TMP_Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        text = textArea.GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(finishExperiment){
            //display result
            text.text = (endTime - startTime).ToString() + "\n" + accuracyResult[0].ToString() + "\n" + accuracyResult[1].ToString() + "\n" + accuracyResult[2].ToString();

        }
    }
}
