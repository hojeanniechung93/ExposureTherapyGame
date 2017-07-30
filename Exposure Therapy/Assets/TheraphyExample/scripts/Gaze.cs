using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour {
	public Transform head;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public RaycastHit getGazeObject(float distance){
		RaycastHit hit;
		//float GazeDistance = 10.0;
		Debug.DrawRay(head.position, head.position + head.forward*distance);
		Ray forwardRay = new Ray (head.position, head.forward);
		Physics.Raycast (forwardRay, out hit, distance);
		return hit;
	}
}
