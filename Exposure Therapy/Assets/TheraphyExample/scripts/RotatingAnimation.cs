using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler (0, 1, 0) * transform.rotation;
	}
}
