using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeInteraction : MonoBehaviour {
    public Image BackgroundImage;
    public Color NormalColor;
    public Color HighlightColor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnGazeEnter()
    {
        BackgroundImage.color = HighlightColor;
    }

    public void OnGazeExit()
    {
        BackgroundImage.color = NormalColor;
    }

    public void OnClick()
    {
        Debug.Log("Click Received");
    }
}
