using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GazeTarget : MonoBehaviour {

	[Range(0f,1f)]
	public float enterGazeGrowth;

	[Range(0f,5f)]
	public float endGazeGrowth;

	public int reward;


	public float rotationSpeed;

	public float gazeTimeForPoints;

	public Color activeColor;
	public Color inactiveColor;

	private float animationTime = 0.3f;

	private float deactivateRotationSpeed;
	private Vector3 originalScale;
	private Vector3 enterGazeScale;
	private Vector3 endGazeScale;
	private Vector3 toScale;
	private Vector3 fromScale;

	private float elapsedTime;

	private MeshRenderer meshRenderer;

	private bool canBeTargeted;

	private bool isBeingTargeted;

	private enum State {inactive, activatingGaze, deactivatingGaze, gazeTimer}
	private State currentState;

	bool hasScored;

	// Use this for initialization
	void Start () {
		originalScale = transform.localScale;

		enterGazeScale = originalScale + originalScale*enterGazeGrowth;

		endGazeScale = originalScale + originalScale*endGazeGrowth;

		fromScale = originalScale;
		toScale = originalScale;

		deactivateRotationSpeed = rotationSpeed * -2;

		meshRenderer = GetComponent<MeshRenderer>();

		DeactivateTarget();

		if (reward == 0) {
			reward = 100;
		}

		hasScored = false;
	}
	

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log("EnterGaze");
			EnterGaze();
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			Debug.Log("LeaveGaze");
			LeaveGaze();
		}

		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			Debug.Log("Activate");
			ActivateTarget();
		}

		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			Debug.Log("Deactivate");
			DeactivateTarget();
		}
	}
	// Update is called once per frame
	void FixedUpdate () {

		if(currentState == State.inactive)
		{
		}
		else if(currentState == State.deactivatingGaze)
		{
			transform.Rotate(Vector3.up*deactivateRotationSpeed*Time.fixedDeltaTime, Space.World);
				
			elapsedTime += Time.fixedDeltaTime;
			transform.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime);

			if(elapsedTime > animationTime)
			{
				currentState = State.inactive;
			}
		}
		else 
		{
			transform.Rotate(Vector3.up*rotationSpeed*Time.fixedDeltaTime, Space.World);
	
			if(currentState == State.activatingGaze)
			{
				Debug.Log("activatingGaze...");
				elapsedTime += Time.fixedDeltaTime;
				transform.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime);

				if(elapsedTime > animationTime)
				{
					currentState = State.gazeTimer;
					elapsedTime = 0f;
				}
			}
			else if(currentState == State.gazeTimer)
			{
				elapsedTime += Time.deltaTime;

				if(elapsedTime > gazeTimeForPoints)
				{



					if(!hasScored)
					{
						var audioSource = gameObject.GetComponent<AudioSource>();
                    	audioSource.PlayOneShot(SoundFX.CollectTarget);
						hasScored = true;
						GameManager.Score += reward;
						StartCoroutine(WaitToDeactivate(0.3f));
					}
				}
			}
		}			
	}

	IEnumerator WaitToDeactivate(float delay)
	{
		yield return new WaitForSeconds(delay);
		gameObject.SetActive(false);
	}

	public void EnterGaze()
	{
		if(!canBeTargeted)
			return;

		Debug.Log("ENTER GAZE");
		currentState = State.activatingGaze;

		fromScale = transform.localScale;
		toScale = enterGazeScale;

		elapsedTime = 0f;
	}

	public void LeaveGaze()
	{
		if(!canBeTargeted)
			return;

		currentState = State.deactivatingGaze;

		fromScale = transform.localScale;
		toScale = originalScale;

		elapsedTime = 0f;
	}

	public void ActivateTarget()
	{
		canBeTargeted = true;
		meshRenderer.material.color = activeColor;
	}

	void DeactivateTarget()
	{
		canBeTargeted = false;
		meshRenderer.material.color = inactiveColor;
	}
}
