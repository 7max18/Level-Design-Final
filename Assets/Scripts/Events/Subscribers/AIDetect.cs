using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AIDetect : MonoBehaviour
{
    private UnityAction someListener;

    void Awake()
    {
        someListener = new UnityAction(SomeFunction);
    }

    void OnEnable()
    {
        EventManager.StartListening("test", someListener);       //set with Unity Action and calls SomeFunction()
        EventManager.StartListening("boom", SomeOtherFunction);  //directly calls SomeOtherFunction()
    }

    void OnDisable()
    {
        EventManager.StopListening("test", someListener);
        EventManager.StopListening("boom", SomeOtherFunction);
    }

    void SomeFunction()
    {
        Debug.Log("Some Test Function was called!");
        EventManager.StopListening("test", someListener);
        EventManager.StartListening("test", SomeThirdFunction);
    }

    void SomeOtherFunction()
    {
        Debug.Log("Some Boom Function was called!");
    }

    void SomeThirdFunction()
    {
        Debug.Log("reset someListener to use this function when test trigger broadcast");
    }
}
