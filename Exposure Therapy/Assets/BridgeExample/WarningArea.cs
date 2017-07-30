using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningArea : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        EventManager.TriggerEvent(GameEvent.EnterWarningArea);
    }

    void OnTriggerExit(Collider other)
    {
        EventManager.TriggerEvent(GameEvent.ExitWarningArea);
    }
}
