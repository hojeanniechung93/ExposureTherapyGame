using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationDisplay : MonoBehaviour {
	public Text ConfirmationText;

	// Use this for initialization
	void Start () {
		ConfirmationText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
