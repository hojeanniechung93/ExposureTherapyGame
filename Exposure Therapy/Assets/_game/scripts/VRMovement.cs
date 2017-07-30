using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMovement : MonoBehaviour {

    [Header("Movement Settings")]
    [Range(0.1f, 10f)]
    public float moveSpeed;

    [Range(0.1f, 10f)]
    public float lateralSpeed;

    [Range(0f, 1f)]
    public float trackpadBuffer;

    [Header("VR Rig References")]
    public Transform head;
    public Transform cameraRig;

    [Header("Dev Mode")]
    public bool useKeyboard = false;
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    Vector2 movementDirection;
    Vector3 nextPos;
    Rigidbody rb;

    public enum WalkingSurface
    {
        Grass,

        Wood
    };

    public WalkingSurface currentWalkingSurface = WalkingSurface.Grass;

    Vector2 touchPadInput;

    public AudioSource walkingAudioSource;

    public Vector2 MoveDirection
    {
        get
        {
            return movementDirection;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (GameManager.Player == null)
        {
            Debug.Log("Do not destroy on load");
            GameManager.Player = gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        if (moveSpeed == 0)
        {
            Debug.LogWarning("Movement speed is zero. Player won't move");
        }

        if(lateralSpeed == 0)
        {
            Debug.LogWarning("Lateral movement speed is zero. Player won't move side ways");
        }

        rb = GetComponent<Rigidbody>();

        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            useKeyboard = true;
        }
        else
        {
            useKeyboard = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!useKeyboard)
        {
            // in addition to moving the player - we must move the camera as well
            // this might not be needed on GearVR depending how the GameOject hierarchy looks
            //cameraRig.position = transform.position;

            touchPadInput = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            
            // if using the headset touchpad - revert coordiantes
            if (OVRInput.GetActiveController() == OVRInput.Controller.Touchpad)
            {
                var x = touchPadInput.x;
                touchPadInput.x = touchPadInput.y * -1; // lateral movement
                touchPadInput.y = x; // forward/backward  movement
            }
        }
        else
        {
            // if using the keyboard - aka dev mode
            touchPadInput.x = Input.GetAxis("Horizontal");
            touchPadInput.y = Input.GetAxis("Vertical");

            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

            //var rotationSpeed = 35;
            //if (Input.GetKey(KeyCode.Q))
            //{
            //    transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
            //}
            //if (Input.GetKey(KeyCode.E))
            //{
            //    transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            //}
    }

        SetDirection(touchPadInput);
    }

    private void FixedUpdate()
    {
        // if there is no movement direction - don't calculate movement
        if (movementDirection.y != 0 || movementDirection.x != 0)
        {
            nextPos =
                transform.position // our current position 
                + (head.right * movementDirection.x*lateralSpeed + head.forward * movementDirection.y*moveSpeed) // lateral and forward/backward movement based on head rotation
                * Time.fixedDeltaTime; // times our movement speed 

            //reset up/down movement - we don't want to fly, just move in the current y plane
            nextPos.y = transform.position.y;

            // interpolate next position
            rb.MovePosition(nextPos);

            this.UpdateWalkingAudio(true);
        }
        else
        {
            this.UpdateWalkingAudio(false);
        }
    }

    private void UpdateWalkingAudio(bool play)
    {
        if (play)
        {
            switch (this.currentWalkingSurface)
            {
                case WalkingSurface.Grass:
                    this.walkingAudioSource.clip = SoundFX.GrassWalk;
                    break;

                case WalkingSurface.Wood:
                    this.walkingAudioSource.clip = SoundFX.WoodWalk;
                    break;
            }

            if (!this.walkingAudioSource.isPlaying)
            {
                this.walkingAudioSource.Play();
            }
        }
        else
        {
            this.walkingAudioSource.Stop();
        }
    }

    public void SetDirection(Vector2 direction)
    {
        if (direction.x > trackpadBuffer)
        {
            //Debug.Log("RIGHT");
            movementDirection.x = 1;
        }
        else if (direction.x < -trackpadBuffer)
        {
            //Debug.Log("LEFT");
            movementDirection.x = -1;
        }
        else
        {
            movementDirection.x = 0;
        }

        if (direction.y > trackpadBuffer)
        {
            //Debug.Log("FORWARD");
            movementDirection.y = 1;
        }
        else if (direction.y < -trackpadBuffer)
        {
            //Debug.Log("BACKWARD");
            movementDirection.y = -1;
        }
        else
        {
            movementDirection.y = 0;
        }
    }
}
