using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeArea : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        EventManager.TriggerEvent(GameEvent.EnterBridge);
    }

    void OnTriggerExit(Collider other)
    {
        EventManager.TriggerEvent(GameEvent.ExitBridge);
    }
}
