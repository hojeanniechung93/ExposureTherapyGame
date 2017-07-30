using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubeblasters : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnGazeEnter()
	{
		Debug.Log ("On Gaze Enter");
	}

	public void OnGazeExit()
	{
		Debug.Log ("On Gaze Exit");
	}

	public void OnClick()
	{
		Debug.Log("Click Received");
	}
}
