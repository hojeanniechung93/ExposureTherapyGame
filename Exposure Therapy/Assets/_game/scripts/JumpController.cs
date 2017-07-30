using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    public Rigidbody player;
    public Transform CenterEyeAnchor;
    public Transform reticleTransform;
    public Transform smallJumpReticleTransform;

    public LaunchArcMesh arcScriptPrefab;
    public LaunchArcMesh arcScript;
    public float ReticleMovingSpeed = 0.08f;

    public bool Jumping;

    public bool useKeyboard;

    public float MaxJumpingDistance = 500f;

    public float MinJumpingDistance = 1f;

    public float SmallJumpDistance = 2f;

    public float CurrentPosition;
    
    private bool hadAimedSinceEnable;

    private Vector3 forceVector;
    private Vector3 smallJumpVector;

    public AudioSource JumpingAudioSource;

    private bool jumpzoneEntered = false;

    private bool movingAway = true;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            useKeyboard = true;
        }
        else
        {
            useKeyboard = false;
        }
    }

    // Use this for initialization
    void Start ()
    {
        var arcs = GameObject.FindObjectOfType(typeof(LaunchArcMesh));

        if(arcs == null)
        {
            arcScript = Instantiate(arcScriptPrefab);

            DontDestroyOnLoad((arcScript.gameObject));
            smallJumpReticleTransform.localPosition = Vector3.forward*SmallJumpDistance;
        }
    }

    void OnEnable()
    {   
    }

    void OnDisable()
    {  
    }

    public void OnEnterJumpzone()
    {
        InitializeParameters();
        ReticleMovingSpeed = MaxJumpingDistance / 300;
        ResetParameters();
        jumpzoneEntered = true;
    }

    public void OnExitJumpzone()
    {
        ResetParameters();
        jumpzoneEntered = false;
    }

    void FixedUpdate()
    {
        bool firekeyUp = (useKeyboard && Input.GetKeyUp(KeyCode.Space)) ||
                               (!useKeyboard && OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger));
        bool firekeyPressed = (useKeyboard && Input.GetKey(KeyCode.Space)) ||
                               (!useKeyboard && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger));
        // if firekey not pressed and had aimed since enabled, jump 
        if (!firekeyPressed && hadAimedSinceEnable && jumpzoneEntered && !Jumping)
        {
            Debug.Log("big jump");
            Jump(forceVector);
        }
        else if (!jumpzoneEntered && firekeyUp && !Jumping)
        {
            Debug.Log("small jump");    
            Vector3 origPosition = new Vector3(CenterEyeAnchor.position.x, CenterEyeAnchor.position.y - 3f, CenterEyeAnchor.position.z);
            smallJumpReticleTransform.position = new Vector3(smallJumpReticleTransform.position.x, CenterEyeAnchor.position.y - 3f, smallJumpReticleTransform.position.z);
            smallJumpVector = CalculateBestThrowSpeed(origPosition, smallJumpReticleTransform.position);
            Jump(smallJumpVector);
        }
    }

    void Update()
    {
        // update arc position and rotation
        if (!jumpzoneEntered || Jumping)
        {
            return;
        }
        // When we are inside of the jump zone, always set the jumping to false
        // so that we can do long jump even if the landing is not detected for some reason.
        Jumping = false;
        Vector3 position = CenterEyeAnchor.position;
        arcScript.transform.position = new Vector3(position.x, (float) (position.y-0.5), position.z);
        Quaternion rotation = CenterEyeAnchor.rotation;
        arcScript.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        // update arc spread
        if ((useKeyboard && Input.GetKey(KeyCode.Space)) || (!useKeyboard && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)))
        {
            // update arc firing angle
            forceVector = CalculateBestThrowSpeed(CenterEyeAnchor.position, reticleTransform.position);
            arcScript.Angle = Vector3.Angle(GetVector3HorizontalDirection(forceVector), forceVector);
            //Debug.Log(string.Format("arcScript.Angle: {0}", arcScript.Angle));
            if (movingAway)
            {
                CurrentPosition += ReticleMovingSpeed;
                if (CurrentPosition >= MaxJumpingDistance)
                {
                    movingAway = false;
                }
            }
            else
            {
                CurrentPosition -= ReticleMovingSpeed;
                if (CurrentPosition <= MinJumpingDistance)
                {
                    movingAway = true;
                }
            }
            reticleTransform.localPosition = Vector3.forward * CurrentPosition;
            arcScript.TargetDistance = CurrentPosition;
            if (!arcScript.enabled)
            {
                arcScript.enabled = true;
            }
            hadAimedSinceEnable = true;
        }

    }

    void Jump(Vector3 forceVector)
    {
        ResetParameters();
        JumpingAudioSource.PlayOneShot(SoundFX.JumpStart); 
        player.AddForce(forceVector, ForceMode.VelocityChange);
        Jumping = true;
    }

    public void JumpDone()
    {
        ResetParameters();
        JumpingAudioSource.PlayOneShot(SoundFX.JumpEnd);
        Jumping = false;
    }

    void ResetParameters()
    {
        hadAimedSinceEnable = false;
        arcScript.enabled = false;
        movingAway = true;
        CurrentPosition = 0;
        arcScript.TargetDistance = CurrentPosition;
        reticleTransform.localPosition = Vector3.zero;
    }


    void InitializeParameters()
    {
        if(arcScript == null)
        {
            Debug.LogError("The arcScript is null");
            return;
        }
        arcScript.meshWidth = .4f;
        arcScript.DistanceMultiplier = 3;
        arcScript.Resolution = 150;
    }

    private Vector3 GetVector3HorizontalDirection(Vector3 vector)
    {
        vector.y = 0;
        return vector.normalized;
    }

    private Vector3 CalculateBestThrowSpeed(Vector3 origin, Vector3 target)
    {
        // calculate vectors
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;

        // calculate xz and y
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
        // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
        // so xz = v0xz * t => v0xz = xz / t
        // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
        float t = (Mathf.Sqrt(xz) * 1.75f) / Mathf.PI;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        // create result vector for calculated starting speeds
        Vector3 result = GetVector3HorizontalDirection(toTargetXZ);        // get direction of xz but with magnitude 1
        result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
        result.y = v0y;                                // set y to v0y (starting speed of y plane)

        return result;
    }
}
