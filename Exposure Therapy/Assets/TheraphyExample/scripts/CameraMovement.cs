using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour {
    float MoveSpeed = 1000f;
	int fixed_distance = 0;
	float hoverBuffer=10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
       // CameraGimbal();
		if (SceneManager.GetActiveScene ().buildIndex == 1) {
			Grounded ();
		}	

         /*   if (Input.GetKey("up"))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
            }
            if (Input.GetKey("down"))
            {			
                transform.Translate(Vector3.back * Time.deltaTime * MoveSpeed);
            }
	*/

    }

    void CameraGimbal()
    {
        float rotationSpeed = 5.0f;
        //playertransform = transform; Camera transform = GetComponentInChildren<Camera>();
        //Gimbal should always use a localRotation
        Camera camera = GetComponentInChildren<Camera>();
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        transform.localRotation = Quaternion.Euler(0, mouseX, 0) * transform.localRotation;
        camera.transform.localRotation = Quaternion.Euler(-mouseY, 0, 0) * camera.transform.localRotation;
    }

	void Grounded(){
		RaycastHit hit;
		Vector3 fixedDistance = new Vector3(0,25.0f,0);
		Ray downRay = new Ray (transform.position, Vector3.down);
		Physics.Raycast (downRay, out hit);
		Vector3 hitpoint = hit.point;
		Vector3 desiredY = hitpoint + fixedDistance;
		if (transform.position.y - hitpoint.y > 150.0f) {
			transform.Translate (Vector3.down*Time.deltaTime*2000);
		}

	}

}
