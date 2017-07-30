using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRecorder : MonoBehaviour
{

    public GameObject LoopPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        Vector3 rotation = transform.rotation.eulerAngles;
	        rotation.y += 90;
            Instantiate(LoopPrefab, gameObject.transform.position, Quaternion.Euler(rotation));
	    }
	}
}
