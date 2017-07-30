using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeExit : MonoBehaviour {

    public float followSpeed;
    GameObject Player;
    public float DistanceToPlayerThreshold = 2f;
    public float DistanceToPlayer = 1.5f;
    public float ExitDelay = 0.5f;
    public string SceneName;
    bool onGaze = false;
    float currentDelay;

    Rigidbody rb;
	// Use this for initialization
	void Start () {

        if(Math.Abs(followSpeed) < 0.001f)
        {
            followSpeed = 10f;
        }

        Player = GameManager.Player;    
        UpdatePosition();

        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if(GameManager.Player == null)
        {
            Debug.Log("No Player in GameManager");
            return;
        }
        
        // follow player up to certain offset

        if(Vector3.Distance(Player.transform.position, transform.position) > DistanceToPlayerThreshold)
        {
			//Debug.Log ("Getting distance");
            UpdatePosition();
        }
        transform.LookAt (Player.transform);

        if (onGaze)
        {
            currentDelay += Time.deltaTime;

            if (currentDelay >= ExitDelay)
            {
                SceneManager.LoadScene(SceneName);
            }
        }
	}

    void UpdatePosition()
    {
        var playerPosition = Player.transform.position;
        var newPos = Vector3.MoveTowards(transform.position, Player.transform.position, followSpeed*Time.deltaTime);
        transform.position = newPos;
    }

    public void OnGazeEnter()
    {
        onGaze = true;
        currentDelay = 0f;
    }

    public void OnGazeLeave()
    {
        onGaze = false;
        currentDelay = 0f;
    }
}
