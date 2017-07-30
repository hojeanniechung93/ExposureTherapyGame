using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionDisplay : MonoBehaviour {
	LoadGamePortal getSelected;
	Text selectionTime;

	// Use this for initialization
	void Start () {
		getSelected = FindObjectOfType<LoadGamePortal> ();
		selectionTime = GetComponent<Text> ();

	}
	
	// Update is called once per frame
	void Update () {
		selectionTime.text = "Time to selection" + getSelected.timer;
	}
}
