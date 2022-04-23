using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AIDetectForEventsArgs : MonoBehaviour
{
    private Action<EventParams> someListener1;
    private Action<EventParams> someListener2;


    void Awake()
    {
        someListener1 = new Action<EventParams>(SomeFunction);
        someListener2 = new Action<EventParams>(SomeOtherFunction);
    }

    
    void OnEnable()
    {
        //Register With Action variable
        EventManagerDelParaArg.StartListening("test", someListener1);

        //OR Register Directly to function
        EventManagerDelParaArg.StartListening("boom", SomeOtherFunction);
    }

    void OnDisable()
    {
        //Un-Register With Action variable
        EventManagerDelParaArg.StopListening("test", someListener1);

        //OR Un-Register Directly to function
        EventManagerDelParaArg.StopListening("boom", SomeOtherFunction);
    }

    void SomeFunction(EventParams eventParam)
    {
        Debug.Log("Some Delegate w Parameters Function was called!");
        if (eventParam.param2==3)
        {
            Debug.Log("test had a param2 of 3");
        }
        
    }

    void SomeOtherFunction(EventParams eventParam)
    {
        Debug.Log("Some Other Delegate w Parameters Function was called!");
        if (eventParam.param1 == "joy")
        {
            Debug.Log("test had JOY");
            Debug.Log("and param2 the bool set to default was: " + eventParam.param2);
        }
    }

    void SomeThirdFunction(EventParams eventParam)
    {
        Debug.Log("Some Third Delegate w Parameters Function was called!");
    }
}
