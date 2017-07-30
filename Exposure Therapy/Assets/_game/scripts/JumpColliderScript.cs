using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class JumpColliderScript : MonoBehaviour
{
    public float JumpZoneMaxDistance;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        Debug.Log(string.Format("Entered the Jump! {0}", other.tag));
        JumpController jumpController = other.gameObject.GetComponent<JumpController>();
        jumpController.MaxJumpingDistance = this.JumpZoneMaxDistance;
        jumpController.OnEnterJumpzone();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        Debug.Log("Left the Jump!");
        JumpController jumpController = other.gameObject.GetComponent<JumpController>();
        jumpController.OnExitJumpzone();
    }
}
