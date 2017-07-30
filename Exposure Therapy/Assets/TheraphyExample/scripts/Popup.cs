using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {
    public GameObject InfoScroll;
	// Use this for initialization
	//Currently there is a bug that if you go through the first canvas the gazepointer ring will disappear regardless of whether or not the InfoScroll is set active or not
	//You have to go around the popup for some reason in order for the gaze pointer to stay put
	void Start () {
        InfoScroll.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void OnTriggerEnter(Collider other)
    {
		InfoScroll.SetActive(true);
        
    }

    private void OnTriggerExit(Collider other)
    { 
       InfoScroll.SetActive(false);
    }


}
