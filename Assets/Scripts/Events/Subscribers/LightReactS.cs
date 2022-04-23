using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightReactS : MonoBehaviour
{
    void OnEnable()
    {
        EventManagerDel.StartListening("boom", LightBoom);
        EventManagerDelParaArg.StartListening("test", LightArg);
    }

    void OnDisable()
    {
        EventManagerDel.StopListening("boom", LightBoom);
        EventManagerDelParaArg.StopListening("test", LightArg);
    }

    void LightBoom()
    {
        Debug.Log("In LightBoom, so BoomDel was called");
    }

    void LightArg(EventParams e)
    {
        Debug.Log("test sent and function with parameter eventparams");

            if (e.param1 == "joy")
            {
                Debug.Log("eventparams had joy");
            }
        
    }
}
