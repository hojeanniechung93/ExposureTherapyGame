using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLoop : MonoBehaviour
{

    public AudioSource LoopEnterAudioSource;
    public float LoopRadius = 1f;
    public float LoopWidth = 0.2f;
    public float EdgeRotationSpeed = 0.2f;
    public float LoopRatationSpeed = 0.2f;

    private GameObjectSearcher searcher;
    private List<GameObject> allEdges;

    void Awake()
    {
        searcher = gameObject.GetComponent<GameObjectSearcher>();
        searcher.enabled = true;
    }

    // Use this for initialization
    void Start () {
		GetAllEdges();
        UpdateLoop();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateLoop();
	}

    void FixedUpdate()
    {
        RotateLoop();
    }

    private void RotateLoop()
    {
        if (allEdges == null)
        {
            return;
        }
        float edgeLength = CalculateEdgeLength();
        float sign = 1f;
        foreach (GameObject edge in allEdges)
        {
            edge.transform.Rotate(sign * Vector3.up*EdgeRotationSpeed, Space.Self);
            sign = -sign;
        }
        gameObject.transform.Rotate(Vector3.right *LoopRatationSpeed, Space.Self);
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateLoop();
        }
    }

    private void UpdateLoop()
    {
        if (allEdges == null)
        {
            return;
        }
        float edgeLength = CalculateEdgeLength();
        foreach (GameObject edge in allEdges)
        {
            // for each of the edges, set the local Y scale to edgeLength
            // for each of the edges, set the local X and Z scales to LoopWidth
            Vector3 localScale = edge.transform.localScale;
            localScale.y = edgeLength;
            localScale.x = LoopWidth;
            localScale.z = LoopWidth;
            edge.transform.localScale = localScale;
            // for each of the edges, set the local Z position to LoopRadius
            Vector3 localPosition = edge.transform.localPosition;
            localPosition.z = LoopRadius;
            edge.transform.localPosition = localPosition;
        }
        // update the trigger collider box for the loop
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        Vector3 colliderSize = collider.size;
        colliderSize.y = colliderSize.z = LoopRadius*2.0f + LoopWidth;
        collider.size = colliderSize;
    }

    private float CalculateEdgeLength()
    {
        return 2f*(LoopRadius+0.5f*LoopWidth)*Mathf.Tan(22.5f*Mathf.Deg2Rad);
    }

    private void GetAllEdges()
    {
        allEdges = searcher.actors;
    }

    public void Score(int score)
    {
        LoopEnterAudioSource.PlayOneShot(SoundFX.CollectTarget);
        GameManager.Score += score;
    }
}
