using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingRay : MonoBehaviour {
	float sightlength=10.0f;
	public RaycastHit seen;
	public bool hitinfocube = false;
	GazeHelper gazeHelper;
	GameObject numpad;


	// Use this for initialization
	void Start () {
		gazeHelper = FindObjectOfType<GazeHelper> ();

	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, transform.forward, Color.red);

		if (!gazeHelper.Gaze (100.0f, out seen)) {
			if (numpad != null) {
				numpad.SendMessage ("OnGazeLeave", null, SendMessageOptions.DontRequireReceiver);
			}
			numpad = null;
			hitinfocube = false;
			return;
		}

		if (numpad != seen.collider.gameObject) {
			if (numpad != null) {
				numpad.SendMessage ("OnGazeLeave", null, SendMessageOptions.DontRequireReceiver);

			}
			numpad = seen.collider.gameObject;
			numpad.SendMessage ("OnGazeEnter", null, SendMessageOptions.DontRequireReceiver);
		}

		if ( OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad) || Input.GetKeyDown(KeyCode.A))
		{
			numpad.SendMessage ("OnGazeClick", null, SendMessageOptions.DontRequireReceiver);
		}

		if (seen.collider.name == "Infocube" && hitinfocube == false) {
			hitinfocube = true;
			return;
		}

		hitinfocube = false;
	}
}
