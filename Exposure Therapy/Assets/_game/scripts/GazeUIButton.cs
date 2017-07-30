using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeUIButton : MonoBehaviour {
	
	public Color hoverColor;

	public string buttonValue;
	InputKeeper inputKeeper;
	int number;
	public Text Reflector;

	Image buttonImage;
	Text buttonText;

	

	// Use this for initialization
	void Start () {
		buttonValue = gameObject.name;
		inputKeeper = FindObjectOfType<InputKeeper> ();
		buttonImage = GetComponent<Image>();
		buttonText = GetComponentInChildren<Text>();

		buttonImage.color = Color.white;
		buttonText.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnGazeEnter(){
		buttonImage.color = hoverColor;
		buttonText.color = Color.white;
	}

	public void OnGazeLeave(){
		buttonImage.color = Color.white;
		buttonText.color = Color.black;
		//Logic for the Next Button

	}

	public void OnGazeClick(){

		Debug.Log("ON GAZE UI BUTTON CLICK : "  + buttonValue);
		if (buttonValue.Equals ("Next")) {
			Debug.Log("NEXT");
			inputKeeper.NextAction();

		} 
		else if (buttonValue.Equals ("Previous")) {
			Debug.Log("PREV");
			inputKeeper.PrevAction();
		}
		else if (int.TryParse (buttonValue, out number)) {
			inputKeeper.SetPanelValue(number);
			Reflector.text = buttonValue;
		}
	}
}
