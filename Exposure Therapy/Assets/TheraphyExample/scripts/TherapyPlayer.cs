using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TherapyPlayer : MonoBehaviour {
	float sightlength=10.0f;
	public RaycastHit seen;
	GazeHelper gazeHelper;
	GameObject TargetList;


	// Use this for initialization
	void Start () {
		gazeHelper = FindObjectOfType<GazeHelper> ();

	}

	// Update is called once per frame
	void Update () {
		Debug.DrawRay (transform.position, transform.forward, Color.red);

		if (!gazeHelper.Gaze (100.0f, out seen)) {
			if (TargetList != null) {
				Debug.Log ("Nothing is seen");
				TargetList.SendMessage ("OnGazeLeave", null, SendMessageOptions.DontRequireReceiver);
			}
			TargetList = null;
			return;
		}

		if (TargetList != seen.collider.gameObject) {
			if (TargetList != null) {
				Debug.Log ("What you see changed : " + seen.collider.gameObject.name);
				TargetList.SendMessage ("OnGazeLeave", null, SendMessageOptions.DontRequireReceiver);

			}
			TargetList = seen.collider.gameObject;
			TargetList.SendMessage ("OnGazeEnter", null, SendMessageOptions.DontRequireReceiver);
		}

		if ( Input.GetKeyDown(KeyCode.A) || OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad)) {
			TargetList.SendMessage ("OnGazeClick", null, SendMessageOptions.DontRequireReceiver);
		}



	}
}
