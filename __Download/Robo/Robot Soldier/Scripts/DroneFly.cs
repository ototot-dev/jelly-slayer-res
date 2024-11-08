using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFly : MonoBehaviour {
 
public Transform  BodyDrone  ;                 
private Vector3  targetPointsPos  ;                 // Points for positions 
 
    private float m_MovementValue;         // The current value of the movement .
 
    private float m_Speed = 8.0f;                 // How fast the tank moves forward and back.
    private bool m_MoveUp;
    private float Angle;
    // Use this for initialization
    void Start () {
	 
	}
	
	// Update is called once per frame
	void Update () {
 
            var heading = transform.position - targetPointsPos ;

               Vector3 newDir = Vector3.RotateTowards(BodyDrone.forward, new Vector3(0,2,0), 10, 0.0F);
 
                BodyDrone.LookAt(new Vector3(0,3,0));

            //move forward
             heading.y = 0;  // This is the overground heading.
            if (heading.sqrMagnitude > 2)
            { //if the target is far move otherwise stand
                if (m_MovementValue < 0.2f)
                    m_MovementValue += 0.01f;
                //turn towards  
                Vector3 targetDir = targetPointsPos  - transform.position;
                float step = 1.2f * Time.deltaTime;
                 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
               
           //     newDir.y = 0;
                transform.rotation = Quaternion.LookRotation(newDir);

            }
            else if (m_MovementValue > 0.2f)
                m_MovementValue -= 0.01f;
            else
            {
                m_MovementValue = 0.2f;
                 Angle=Angle+Random.Range(20.0f, 25.0f);

                    float Vx, Vz, DIST;   // y,z components of the initial velocity
                    DIST=Random.Range(2.5f, 5f) ;
                    Vx = DIST * Mathf.Sin(Mathf.Deg2Rad * Angle);
                    Vz = DIST * Mathf.Cos(Mathf.Deg2Rad * Angle);

 

                    targetPointsPos  = new Vector3(Vx,Random.Range(1.7f, 3.5f)  ,Vz);

            }
 

       
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
         Vector3 movement = transform.forward * m_MovementValue * m_Speed * Time.deltaTime;

 
        transform.position = new Vector3(transform.position.x+ movement.x, transform.position.y +movement.y, transform.position.z + movement.z);
  


    }
}
