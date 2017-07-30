﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
	Text Score;
	// Use this for initialization
	void Start () {
		Score = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		Score.text = "Score: "+GameManager.Score;

	}
}
