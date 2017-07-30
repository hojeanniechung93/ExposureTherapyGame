using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GazeHelper : MonoBehaviour {

    public Transform head;
    public float distance;


    private Ray ray;
    
    private bool hasHit;
    private RaycastHit hitpoint;


    private void Start()
    {

    }

    private void Update()
    {

    }
    // Use this for initialization
    public bool Gaze(float distance, out RaycastHit hit)
    {
        ray = new Ray(head.position, head.forward);

        hasHit = Physics.Raycast(ray, out hit, distance);

        return hasHit;
    }

    public bool Gaze(float distance, out RaycastHit hit, int layerMask)
    {
        ray = new Ray(head.position, head.forward);

        hasHit = Physics.Raycast(ray, out hit, distance, layerMask);

        return hasHit;
    }
}
