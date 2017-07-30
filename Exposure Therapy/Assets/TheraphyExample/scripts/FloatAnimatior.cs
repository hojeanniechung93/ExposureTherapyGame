using UnityEngine;
using System.Collections;

public class FloatAnimatior : MonoBehaviour {
   public float floatStrength = 1;
   public float amplitude = 0.05f;

    private Vector3 pos;
	// Use this for initialization
	void Start () {
        pos = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        Animate();
	}

    void Animate()
    {
        transform.position = new Vector3(pos.x,pos.y+amplitude*Mathf.Sin(Time.time)*floatStrength,pos.z);
       
    }
}
