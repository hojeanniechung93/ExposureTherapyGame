using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGamePortal : MonoBehaviour {
	public GameObject Portal;
	public float timer = 1.0f;
	float tempStart;
	bool isTherapy;
	Color prevColor;
	int indexAdd = 0;
	bool doOnce=false;
	ConfirmationDisplay selected;
	CastingRay castingray;

	// Use this for initialization
	void Start () {
		tempStart = timer;
		selected = FindObjectOfType<ConfirmationDisplay> ();
		castingray = FindObjectOfType<CastingRay>();
	}
	
	// Update is called once per frame
	void Update () {
		//Gotta fix this logic so that this is not counted as an event for a random click. Maybe it will be something fixed with the onTriggerEnter

		if (castingray.hitinfocube == true) {
			if (timer > 0) {
				timer -= Time.deltaTime;

			}
			if (timer < 0) {
				
				if (doOnce == false) {
					onMouseClick ();
					indexAdd++;
				}

			}
		} else {
			doOnce = false;
			timer = tempStart;
			selected.ConfirmationText.text = "Not Selected";

		}

	}


	public void onMouseClick(){
		Debug.Log ("clicked");
		doOnce = true;
		selected.ConfirmationText.text = "Selected!";
		LevelManager levelManager = FindObjectOfType<LevelManager> ();
		//levelManager.indexskip = levelManager.indexskip + indexAdd % 2;

	}

	void Reset(){
		if (castingray.hitinfocube != true) {
			print("Reset initiated");
			this.gameObject.SetActive (true);
			timer = tempStart;
			selected.ConfirmationText.text = "Not Selected";
		}
	}
}
