using UnityEngine;
using System.Collections;

public class CubeEnabler : MonoBehaviour {
	public GameObject parentCube;
	public Vector3 PrevScale;
	public Vector3 PostScale=new Vector3(1f,1f,1f);
	Color PrevColor=Color.gray;
	Color PostColor=Color.red;
	public bool RotationEnabled = false;
	// Use this for initialization
	void Start () {
	//Make sure the color and size is the same as OnTriggerExit
		foreach(Transform child in transform.root){
			if (child.gameObject.name.Equals ("Cube")) {
				child.GetComponent<Renderer> ().material.color = Color.gray;
				PrevScale = child.GetComponent<Transform> ().transform.localScale;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (RotationEnabled == true) {
			foreach(Transform child in transform.root){
				child.transform.rotation = child.transform.rotation * Quaternion.Euler (0, 1, 0);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		print ("triggered");
		if (other.gameObject.name.Equals("PlayerPrefab")) {
			foreach(Transform child in transform.root){
				if (child.gameObject.name.Equals ("Cube")) {
					child.GetComponent<Renderer> ().material.color = Color.red;
					child.GetComponent<Transform> ().transform.localScale = PostScale;
					RotationEnabled = true;

				}
			}
		}
	

	}

	private void OnTriggerStay(Collider other){
		OnTriggerEnter (other);
	}



}
