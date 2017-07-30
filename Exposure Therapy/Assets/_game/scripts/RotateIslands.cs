using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIslands : MonoBehaviour {

	[Range(5f, 20f)]
	public float maxRotationSpeed;

	[Range(2f, 5f)]
	public float minRotationSpeed;

	Dictionary<Transform, float> islands;

	// Use this for initialization
	void Start () {
		int count = transform.childCount; 
		islands = new Dictionary<Transform, float>();

		foreach(Transform child in transform)
		{
			islands.Add(child, Random.Range(minRotationSpeed, maxRotationSpeed) * (Random.Range(-1,1) == 0 ? 1 : -1 ));
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		foreach(var island in islands.Keys)
		{
			island.Rotate(Vector3.up*islands[island]*Time.fixedDeltaTime, Space.World);
		}
	}
}
