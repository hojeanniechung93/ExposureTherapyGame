using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetActivator : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
		foreach (Transform child in transform)
        {
            child.SendMessage("ActivateTarget", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider other)
    {
        foreach (Transform child in transform)
        {
            child.SendMessage("DeactivateTarget", null, SendMessageOptions.DontRequireReceiver);
        }
    }
}
