using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 *Right now the function is set up so that when you enter the infobubble and stay in it for a certain timeframe it will load you to the next scene that is loaded 
 *onto the script.
 */

public class BonusLevelPortal : MonoBehaviour {
	public float stayTime = 2f;
	private float timer = 0f;
	private bool bubbleEntered=false;
	public Scene levelScene;
	public string LevelSceneName;
	public GameObject PortalTitle;
    public GameObject currentSpawnPoint;

    int SCORE_THRESHOLD = 100;

	public NextSceneConfigurator NextSceneConfigurator = null;

    // Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		//If control boolean is true then watch for a user click to bring them to the next level
	    if (bubbleEntered)
	    {
	        //Check if the user has enough score
	        timer += Time.deltaTime;
	        float timediff = stayTime - timer;
	        PortalTitle.GetComponentInChildren<Text>().text = "Please wait " + timediff + "until we transport you.";

	        if (timer >= stayTime)
	        {
	            PortalTitle.GetComponentInChildren<Text>().text = "Transporting";

	            // resetting player score to 0
	            GameManager.Score = 0;

                Debug.Log("setting completed 1");
                GameManager.SetLevelCompleted(GameManager.CurrentSceneName);
                GameManager.SetCheckpoint(GameManager.CurrentSceneName, currentSpawnPoint);

	            if (!string.IsNullOrEmpty(LevelSceneName))
	            {
	                if (NextSceneConfigurator != null)
	                {
	                    NextSceneConfigurator.ConfigureNextScene();
	                }

	                SceneManager.LoadScene(LevelSceneName);
	                timer = 0f;
	            }
	            else
	            {
	                SceneManager.LoadScene(levelScene.name);
	                timer = 0f;
	            }
	        }
	    }
	}

	private void OnTriggerEnter(Collider other)
	{
		//Set control boolean to true
		Debug.Log("trigger enter");
		bubbleEntered=true;
	}

	private void OnTriggerStay(Collider other){
		OnTriggerEnter (other);
	}

	private void OnTriggerExit(Collider other)
	{ 
		PortalTitle.GetComponentInChildren<Text> ().text = "Therapy";
		// Set control boolean to false
		bubbleEntered=false;
	}

}
