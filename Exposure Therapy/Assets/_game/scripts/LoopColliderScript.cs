using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopColliderScript : MonoBehaviour
{

    private JumpLoop loop;

    void Start()
    {
        loop = gameObject.GetComponent<JumpLoop>();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.name);
        if (col.CompareTag("Player"))
        {
            Debug.Log("You scored!");
            // add points here
            loop.Score(100);
        }
    }

}
