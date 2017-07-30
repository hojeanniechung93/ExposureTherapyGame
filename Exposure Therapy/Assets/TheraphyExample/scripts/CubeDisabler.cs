using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDisabler : MonoBehaviour {
	CubeEnabler cubeEnabler;

	// Use this for initialization
	void Start () {
		cubeEnabler = FindObjectOfType<CubeEnabler> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{ 
		if (other.gameObject.name.Equals("PlayerPrefab")) {
			foreach(Transform child in transform.root){
				print ("Is this hitting?");
				if (child.gameObject.name.Equals ("Cube")) {
					child.GetComponent<Renderer> ().material.color = Color.gray;
					child.GetComponent<Transform> ().transform.localScale = cubeEnabler.PrevScale;
					cubeEnabler.RotationEnabled = false;
				}
			}
		}
	}
}
