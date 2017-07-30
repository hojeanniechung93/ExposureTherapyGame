using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRemainingDisplay : MonoBehaviour {
	GazeTarget gazeTarget;
	Text timeRemaining;
	// Use this for initialization
	void Start () {
		gazeTarget = FindObjectOfType<GazeTarget> ();
		timeRemaining = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		timeRemaining.text = "Score Timer: ";

	}
}