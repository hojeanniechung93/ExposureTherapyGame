using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCoin : MonoBehaviour {
	public GameObject WinnerMessage;
	public GameObject WinnerModel;
    public GameObject CompletedGameFox;

	// Use this for initialization
	void Start () {
		WinnerMessage.SetActive (false);
		WinnerModel.SetActive (false);
        CompletedGameFox.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		AudioSource songs = WinnerModel.GetComponentInChildren<AudioSource> ();
		songs.clip=SoundFX.EndScene;
		songs.Play ();
		gameObject.SetActive (false);
		WinnerMessage.SetActive (true);
		WinnerModel.SetActive (true);
	    if (GameManager.AreAllLevelsCompleted())
	    {
	        CompletedGameFox.SetActive(true);
	    }
	}
}
