using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSC_Cam : MonoBehaviour {
	public Transform target;
	private Vector3 pos;
	// Use this for initialization
	void Start () {
		pos = transform.position-target.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (transform.position, pos+target.position, 0.5f * Time.deltaTime);
 
	}
}
