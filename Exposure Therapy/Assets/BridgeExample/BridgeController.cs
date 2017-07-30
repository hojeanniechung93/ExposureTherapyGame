using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class BridgeController : MonoBehaviour
{
    public float warningFadeTime;
    public float gazeMaxDistance;    
    OVRFadeHelper fadeHelper;
    GazeHelper gazeHelper;
    RaycastHit hit;

    GameObject lastTargetHit;

    private bool isOnBridge;

    public AudioSource environmentAudioSource;
    private VRMovement vrMovement;

    void Start ()
    {
        this.vrMovement = gameObject.GetComponent<VRMovement>();

        fadeHelper = FindObjectOfType<OVRFadeHelper>();
        if (fadeHelper == null)
        {
            Debug.LogWarning("BridgeController on scene without a screen transition. Make sure add OVRFadeHelper to the camera object in the OVR game object.");
        }

        gazeHelper = FindObjectOfType<GazeHelper>();
        if (gazeHelper == null)
        {
            Debug.LogWarning("BridgeController on scene without a gazeHelper.");
        }

        isOnBridge = true;

        environmentAudioSource = gameObject.transform.FindChild("EnvironmentAudioSource").GetComponent<AudioSource>();;

    }
	
    public void OnEnable()
    {
        EventManager.StartListening(GameEvent.EnterWarningArea, () => ShowWarning());
        EventManager.StartListening(GameEvent.ExitWarningArea, () => StopShowingWarning());

        EventManager.StartListening(GameEvent.EnterBridge, () => EnterBridge());
        EventManager.StartListening(GameEvent.ExitBridge, () => ExitBridge());

    }

    public void OnDisable()
    {
        EventManager.StopListening(GameEvent.EnterWarningArea, () => ShowWarning());
        EventManager.StopListening(GameEvent.ExitWarningArea, () => StopShowingWarning());

        EventManager.StopListening(GameEvent.EnterBridge, () => EnterBridge());
        EventManager.StopListening(GameEvent.ExitBridge, () => ExitBridge());
    }

    private void ShowWarning()
    {
        if(SoundFX.WarningWind == null)
        {
            Debug.LogError("Warning wind is null");
            return;
        }

        this.environmentAudioSource.clip = SoundFX.WarningWind;
        this.environmentAudioSource.Play();
    }

    private void StopShowingWarning()
    {
        StartCoroutine(fadeHelper.FadeTo(fadeHelper.InvisibleScreenColor, warningFadeTime, turnOffOverlayAtTheEnd: true));

        this.environmentAudioSource.clip = SoundFX.Wind;
        this.environmentAudioSource.Play();
    }

    private void EnterBridge()
    {
        Debug.Log("Entered Bridge");

        this.vrMovement.currentWalkingSurface = VRMovement.WalkingSurface.Wood;
    }

    private void ExitBridge()
    {
        Debug.Log("Exited Bridge");

        this.vrMovement.currentWalkingSurface = VRMovement.WalkingSurface.Grass;
    }

    void FixedUpdate()
    {
        if(!isOnBridge)
            return;

        if(gazeHelper.Gaze(gazeMaxDistance, out hit))
        {
            if(lastTargetHit != hit.collider.gameObject)
            {
                // is a new target
                // set previous target to white
                if(lastTargetHit != null)
                {
                    lastTargetHit.gameObject.SendMessage("LeaveGaze", null, SendMessageOptions.DontRequireReceiver);
                }

                // update current target color
                lastTargetHit = hit.collider.gameObject;
                lastTargetHit.gameObject.SendMessage("EnterGaze", null, SendMessageOptions.DontRequireReceiver);
            }
        }
        else if( lastTargetHit != null)
        {
            lastTargetHit.gameObject.SendMessage("LeaveGaze", null, SendMessageOptions.DontRequireReceiver);
            lastTargetHit = null;
        }
    }
}
