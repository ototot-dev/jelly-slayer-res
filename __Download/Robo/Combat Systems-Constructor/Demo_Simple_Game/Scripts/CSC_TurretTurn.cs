using UnityEngine;
using System.Collections;

public class CSC_TurretTurn : MonoBehaviour
{
	//This script was created for use in the resource "Combat Systems-Constructor", 
	//the script is not perfect;), if you find errors in it or improve, 
	//please share information with me. Thanks.

	public Transform HorizontalAxis;
	public Transform VerticalAxis;

	public string EnemiesTag;

	private Vector3 Target;

	// in degrees
	//public float leftExtent=-90;
	public float range=300;
	public float SpeedTurn=50;
	public float HorizontalConstraint=90;
	public float UpConstraint=90;
	public float DownConstraint=-90;

	public weap[] Weap;
	[System.Serializable]
	public struct  weap {
		public Transform Weapon;
		public float FireRate;	//1			// Number in seconds which controls how often the player can fire


	}
	private Animator animatorWeap;
	private float[] nextFire;                     // Float to store the time the player will be allowed to fire again, after firing
	private Vector3 dirToTarget;
	private Quaternion newRotationX,newRotationY ;
	private float TargetDistance;
	private bool TargetON; //is there Target 
    private Quaternion HorizontalAxis_v,VerticalAxis_v;
	[SerializeField, HideInInspector]
	Quaternion	original,originalBarrel;
	private Vector3 dirXZ, forwardXZ, dirYZ, forwardYZ;
	//The search for the target will be carried out with the help of cortex every 0.1 
	private float searchTimeDelay = 0.1f;
	// Use this for initialization
 
	void Start ()
	{
		
		originalBarrel =  VerticalAxis.transform.rotation;
		StartCoroutine(FindClosestTarget());
		if(EnemiesTag=="") print("No NameEnemiesTag !");
		System.Array.Resize(ref nextFire, Weap.Length);

		HorizontalAxis_v = HorizontalAxis.transform.rotation;
		original = Quaternion.Euler ( HorizontalAxis_v.eulerAngles.x,  0,0); 
 

	}

	// Update is called once per frame
	void LateUpdate ()
	{


		// HorizontalAxis.localRotation = original;
		if ( !TargetON) {

			HorizontalAxis_v  = Quaternion.RotateTowards(HorizontalAxis_v ,HorizontalAxis.transform.rotation , SpeedTurn/10);
		 		HorizontalAxis.rotation  = HorizontalAxis_v ;
            VerticalAxis_v = Quaternion.RotateTowards(VerticalAxis_v, VerticalAxis.transform.rotation, SpeedTurn / 10);
            VerticalAxis.transform.rotation = Quaternion.Euler(VerticalAxis_v.eulerAngles.x, VerticalAxis.eulerAngles.y, 0);

            return;
		}


		dirToTarget = (Target - HorizontalAxis.transform.position);
		 
 
		Vector3 yAxis = Vector3.up; // world y axis
		dirXZ = Vector3.ProjectOnPlane (dirToTarget, yAxis);
        forwardXZ = Vector3.ProjectOnPlane(transform.forward, yAxis);

        float parentY = transform.rotation.eulerAngles.y;
		if(parentY<0) parentY=-parentY;
		if(parentY>180) parentY-=360;

        float yAngle = parentY + Vector3.Angle(dirXZ, forwardXZ) * Mathf.Sign(Vector3.Dot(yAxis, Vector3.Cross(forwardXZ, dirXZ)));
        float yClamped = Mathf.Clamp(yAngle, parentY - HorizontalConstraint, parentY + HorizontalConstraint);
        Quaternion yRotation = Quaternion.AngleAxis(yClamped, Vector3.up);

        Quaternion xRotation=  Quaternion.Euler(0,0,0);
		float xClamped=0;
		float xAngle=0;
		if(yClamped==yAngle){
            dirToTarget = (Target - VerticalAxis.transform.position);
            Vector3 originalForward = yRotation * new Vector3(0, 0, 1);
            Vector3 xAxis = yRotation * Vector3.right; // our local x axis
            dirYZ = Vector3.ProjectOnPlane(dirToTarget, xAxis);
            forwardYZ = Vector3.ProjectOnPlane(originalForward, xAxis);
            xAngle = Vector3.Angle(dirYZ, forwardYZ) * Mathf.Sign(Vector3.Dot(xAxis, Vector3.Cross(forwardYZ, dirYZ)));
            xClamped = Mathf.Clamp(xAngle, -UpConstraint, -DownConstraint);
            xRotation = Quaternion.AngleAxis(xClamped, Vector3.right);
        }

        if (HorizontalAxis.transform == VerticalAxis.transform)
        {
            newRotationX = yRotation * original * xRotation;
            HorizontalAxis_v = Quaternion.RotateTowards(HorizontalAxis_v, newRotationX, SpeedTurn / 10);
        }
        else
        {
            newRotationX = yRotation * original;
            HorizontalAxis_v = Quaternion.RotateTowards(HorizontalAxis_v, newRotationX, SpeedTurn / 10);
            newRotationY = originalBarrel * xRotation;
            VerticalAxis_v = Quaternion.RotateTowards(VerticalAxis_v, newRotationY, SpeedTurn / 10);
            VerticalAxis.transform.rotation = Quaternion.Euler(VerticalAxis_v.eulerAngles.x, VerticalAxis.eulerAngles.y, 0);
        }
        HorizontalAxis.rotation = HorizontalAxis_v;


        //fire

        if (xClamped==xAngle && yClamped==yAngle && TargetDistance<range-(range/4) && HorizontalAxis_v.eulerAngles ==newRotationX.eulerAngles  )
		for (int i = 0; i < Weap.Length ; i++){
			if (nextFire[i] <= 0) {
			 
				 nextFire[i] = Weap[i].FireRate;
                 animatorWeap = Weap[i].Weapon.GetComponent<Animator> ();
				 animatorWeap.SetTrigger("Fire" );

			}else 
			if (nextFire[i] > 0) {
				nextFire[i] -= 0.01f;
			 
			}
		}

	}
	protected virtual IEnumerator FindClosestTarget()
	{
		while(true)
		{
			
			//The nearest target caught in the radius of the review
			 TargetDistance=range*2;
	 
			Vector3 closest = new Vector3();
            TargetON = false;
            GameObject[] targets = GameObject.FindGameObjectsWithTag(EnemiesTag);
			foreach (GameObject obj in targets)
			{
				 
				//Find the distance between the turret and the intended target
				Vector3 diff = obj.transform.position - transform.position;
				float	nearest = diff.sqrMagnitude;
	
				//	If a target is found in the radius of the lesion, then we remember it
				if (nearest < range && nearest<TargetDistance)
				{
					TargetDistance=nearest ;
                    closest  =   obj.transform.position+ new Vector3(0, GetComponent<BoxCollider>().size.y/2, 0);// target to middle of unit
                    TargetON = true;
                }
			}
	
			Target = closest  ;
 
			yield return new WaitForSeconds(searchTimeDelay);
		}
	}
 
	void OnDrawGizmos ()
	{
	/*	
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (HorizontalAxis.transform.position, HorizontalAxis.transform.position + dirXZ);
		Gizmos.DrawLine (HorizontalAxis.transform.position, HorizontalAxis.transform.position + forwardXZ);
		Gizmos.color = Color.green;
		Gizmos.DrawLine (HorizontalAxis.transform.position, HorizontalAxis.transform.position + dirYZ);
	 	Gizmos.DrawLine (HorizontalAxis.transform.position, HorizontalAxis.transform.position + forwardYZ);
		*/
	}
}