using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {


    [Header("Player Settings")]
    public Transform SpawnPoint;
    public GameObject player;

    [Header("Fade Settings")]
    public float OutTime;
    public float InTime;
    public Color fadeOut = new Color(0.01f, 0.01f, 0.01f, 1.0f);
    public Color fadeIn = new Color(0.01f, 0.01f, 0.01f, 0.0f);

    OVRFadeHelper fadeHelper;

    private YieldInstruction fadeInstruction = new WaitForEndOfFrame();

    void Start()
    {
        fadeHelper = FindObjectOfType<OVRFadeHelper>();

        if(fadeHelper == null)
        {
            Debug.LogWarning("KillZone on scene without a screen transition. Make sure add OVRFadeHelper to the camera object in the OVR game object.");
        }

        player = GameManager.Player;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("You died!");
        StartCoroutine(RespawnTransition(other.GetComponent<Rigidbody>()));
    }

    IEnumerator RespawnTransition(Rigidbody playerRigidBody)
    {
        StartCoroutine(fadeHelper.FadeTo(fadeOut, OutTime, turnOffOverlayAtTheEnd: false));
        float elapsedTime = 0.0f;
        while (elapsedTime < OutTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
        }

        GameManager.Player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        Vector3 point;
        if(!GameManager.GetCurrentCheckPoint(out point))
        {
            Debug.LogError("NO CURRENT CHECKPOINT - what whaaaaa");
        }

        GameManager.Player.transform.position = point;
        StartCoroutine(fadeHelper.FadeTo(fadeIn, InTime, turnOffOverlayAtTheEnd: true));
    }
}
