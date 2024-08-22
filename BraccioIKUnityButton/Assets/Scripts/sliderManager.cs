using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;

public class sliderManager : MonoBehaviour
{
    public GameObject HandTrackerManager;
    private HandTrackerForUR handTracker;
    // Start is called before the first frame update
    void Start()
    {
        handTracker = HandTrackerManager.GetComponent<HandTrackerForUR>();
    }

    // OnSliderUpdated 
    public void DebugOnSliderUpdated(SliderEventData eventData)
    {
        Debug.Log("OnValueUpdated Event:" + eventData.NewValue);
    }

    // OnInteractionStarted 
    public void DebugOnInteractionStarted(SliderEventData eventData)
    {
        
    }

    // OnInteractionEnded 
    public void DebugOnInteractionEnded(SliderEventData eventData)
    {
        Debug.Log("OnInteractionEnded Event:" + eventData.NewValue);
        handTracker.adjust = eventData.NewValue * 10f;
    }

    // OnHoverEntered 
    public void DebugOnHoverEntered(SliderEventData eventData)
    {
        
    }

    // OnHoverExited 
    public void DebugOnHoverExited(SliderEventData eventData)
    {
       
    }
}
