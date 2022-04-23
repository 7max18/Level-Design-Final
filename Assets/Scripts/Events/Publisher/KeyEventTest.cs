using UnityEngine;
using System.Collections;

public class KeyEventTest : MonoBehaviour
{


    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            EventManager.TriggerEvent("test");
 
            EventManagerDelParaArg.TriggerEvent("test", new EventParams(null, 0));

        }

        if (Input.GetKeyDown("s"))
        {
            EventManager.TriggerEvent("boom");

            EventManagerDel.TriggerEvent("boom");
        }

        if (Input.GetKeyDown("d"))
        {
            EventManager.TriggerEvent("Junk");

            EventParam myparams = default(EventParam);
            myparams.param1 = "joy";
            myparams.param2 = 3;
            myparams.param3 = 2;
           // myparams.param4 = true;


            EventManagerDelPara.TriggerEvent("boom",myparams);
            EventManagerDelPara.TriggerEvent("test", myparams);
        }

    }
}

