using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllersInfoHUD : MonoBehaviour {

    public GameObject ControllersHUD;

	// Update is called once per frame
	void Update () {
		
        if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            ControllersHUD.SetActive(!ControllersHUD.activeSelf);
        }
	}
}
