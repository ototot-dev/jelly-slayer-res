using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CSC_CharacterViewer : MonoBehaviour {
	//Script control of the camera in the table animation
	public Transform cameras;

    public Transform targetForCamera;
	Vector3 deltaPosition;
	Vector3 lastPosition = Vector3.zero;
	bool rotating = false;

	void Awake () {
		transform.Rotate(0, -90, 0); 
        deltaPosition = cameras.position - targetForCamera.position;
		deltaPosition = new Vector3(deltaPosition.x, deltaPosition.y - (-0.3f) * 5, deltaPosition.z +(-0.3f) * 8);
 
		transform.Rotate(0, -300f * -170 / Screen.width, 0);
    }

	void Update () {
		var y = 0f;
		if (Input.GetMouseButtonDown (0) && Input.mousePosition.x > Screen.width*0.25 ) {
			lastPosition = Input.mousePosition;
			rotating = true;
		}
		if (Input.GetMouseButtonDown (1) && Input.mousePosition.y > Screen.height*0.25 ) {
			 
		 lastPosition  = Input.mousePosition;
		 	rotating = true;

		}

		if (Input.GetMouseButtonUp(0))
			rotating = false;

        if (rotating && Input.GetMouseButton(0))
        {
            transform.Rotate(0, -300f * (Input.mousePosition - lastPosition).x / Screen.width, 0);
 
         }

        if (rotating && Input.GetMouseButton(1))
        {
			cameras.transform.position= new Vector3(cameras.transform.position.x,cameras.transform.position.y-1, cameras.transform.position.z);
 
         }

        if (Input.GetAxis("Mouse ScrollWheel")!=0)
        {

        
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && deltaPosition.y < 18) y = Input.GetAxis("Mouse ScrollWheel");
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && deltaPosition.y > 10f) y = Input.GetAxis("Mouse ScrollWheel");
           deltaPosition = new Vector3(deltaPosition.x, deltaPosition.y - y * 5, deltaPosition.z + y * 8);
        }


        lastPosition = Input.mousePosition;
	}

	void LateUpdate () {
	 	cameras.position += (targetForCamera.position + deltaPosition - cameras.position) * Time.unscaledDeltaTime * 5;
		Vector3 relativePos = targetForCamera.position - cameras.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        cameras.rotation = rotation;
    }
}
