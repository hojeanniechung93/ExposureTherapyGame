using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeInput_Example : MonoBehaviour {

    public float gazeMaxDistance;
    private GazeHelper gazeHelper;
    public bool isGazeActive;

    public Transform GazePointVisualHelper;

    private RaycastHit hit;

    private GameObject lastTargetHit;

    // Use this for initialization
    void Start () {

        gazeHelper = FindObjectOfType<GazeHelper>();
	}
	
	// Update is called once per frame
	void Update () {

        // KeyCode - Space
        // gaze only works while space is pressed
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isGazeActive = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isGazeActive = false;
        }

        // KeyCode T - toggle gaze on off
        if (Input.GetKeyDown(KeyCode.T))
        {
            isGazeActive = !isGazeActive;
        }

        if (isGazeActive)
        {
            if(gazeHelper.Gaze(gazeMaxDistance, out hit))
            {
                if(hit.collider.gameObject.name == "Cube")
                {
					Debug.Log ("printing");
                    if(lastTargetHit != hit.collider.gameObject)
                    {
                        GazePointVisualHelper.gameObject.SetActive(true);
                        // is a new target
                        // set previous target to white
                        SetTargetColor(Color.white);
                        
                        // update current target color
                        lastTargetHit = hit.collider.gameObject;
                        SetTargetColor(Color.blue);
                    }
                    GazePointVisualHelper.position = hit.point;
                }
            }
            else if(lastTargetHit != null)
            {
                GazePointVisualHelper.gameObject.SetActive(false);
            
                // is using gaze, but it didn't hit anything
                SetTargetColor(Color.white);
                lastTargetHit = null;
            }
        }
        else if(lastTargetHit != null)
        {
            GazePointVisualHelper.gameObject.SetActive(false);

            // not pressing hit key and we had a last target
            // return the color target to white and set it to null
            SetTargetColor(Color.white);
            lastTargetHit = null;
        }

	}

    void SetTargetColor(Color color)
    {
        if(lastTargetHit == null)
        {
            return;
        }

        lastTargetHit.GetComponent<MeshRenderer>().material.color = color;
    }
}
