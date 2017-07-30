using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetTriggerColliderScript : MonoBehaviour
{

    public JumpController jumpController;

    void Awake()
    {
        jumpController = gameObject.GetComponentInParent<JumpController>();
    }

    void OnEnable()
    {
        Debug.Log(string.Format("landing trigger collider enabled!!!"));
    }

    void OnDisable()
    {
        Debug.Log(string.Format("landing trigger collider disabled!!!"));
    }

    // OnTriggerEnter will be called regardless of the script being enabled or not.
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("tag: {0}", other.tag));
        // TODO: for now we don't distinguish between island colliders and bridge
        // TODO: edge colliders
        if (other.CompareTag("loop") || other.CompareTag("loopedge"))
        {
            return;
        }
        if (jumpController.Jumping)
        {
            // landed
            Debug.Log(string.Format("player landed!!!"));
            jumpController.JumpDone();
            enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // flying
        if (jumpController.Jumping)
        {
            enabled = true;
        }
    }
}
