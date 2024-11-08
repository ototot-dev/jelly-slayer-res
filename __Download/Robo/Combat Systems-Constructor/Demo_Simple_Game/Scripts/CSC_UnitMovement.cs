


using UnityEngine;
using System.Collections;

	public class CSC_UnitMovement : MonoBehaviour
	{
		public float m_Speed = 3.0f;                 // How fast the tank moves forward and back.
		public float m_TurnSpeed = 1f;            // How fast the tank turns in degrees per second.
		public int currentHealth = 50;  			//The tank's current health point total
 
 		public AudioClip tankDead;   
		public ParticleSystem m_ExplosionParticles;      //  the particles that will play on explosion.
		public ParticleSystem m_SmokeEffect;
		public int DeadMax=4;						//the number of death animations
		private Rigidbody m_Rigidbody;              // Reference used to move the tank.
	 
		private AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds.

		private WaitForSeconds shotDuration = new WaitForSeconds(15f);    // WaitForSeconds hide object 
   
 
		private float posY;
		private bool m_dead;

		private Animator animator;
		private string animName,animNameOld;
		private int waitPointMove=500; //waiting time at point of movement

 
 				
		public TargetPointsPos[] targetPointsPos;//(AI) Points for positions and commands
		[System.Serializable]
		public struct  TargetPointsPos {
			public Transform pos;
			public string act;	//1			// Number in seconds which controls how often the player can fire
		}
		private byte sel_ltargetPointPos; 					//(enemy AI) selected targetPointPos in array
 
		private void Awake ()
		{
		animator =  GetComponent<Animator> ();
		m_Rigidbody = GetComponent<Rigidbody> ();
 		animName="Idle";
 
		}
 


		private void Update ()
		{
			if (m_dead)
				return;
 


			//////////////// for   AI //////////////// begin
 
				if (targetPointsPos.Length > 0) {
					var heading = transform.position - targetPointsPos [sel_ltargetPointPos].pos.position;

				 
					//move forward
			if (heading.sqrMagnitude > 50) { //if the target is far move otherwise stand
				animName = "MoveForward";
				//turn towards  
				Vector3 targetDir = targetPointsPos [sel_ltargetPointPos].pos.position - transform.position;
				float step = m_TurnSpeed * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);	
				newDir.y = 0;
				transform.rotation = Quaternion.LookRotation (newDir);
							


			} else {
				//The tank got to the target, choose another target position for movement
				if (targetPointsPos.Length > 1)
				if (targetPointsPos [sel_ltargetPointPos].act == "" || waitPointMove == 0) { 
					waitPointMove = 500;

					if (sel_ltargetPointPos < targetPointsPos.Length - 1)
						sel_ltargetPointPos++;
					else
						sel_ltargetPointPos = 0;
				} else {
					if (targetPointsPos [sel_ltargetPointPos].act == "wait" ) { 
						if (waitPointMove > 0)		waitPointMove--; 
						animName="Idle";
					}
				}
			}
				}
			//////////////// for   AI //////////////// end
 

		if (animName != animNameOld) {
			animNameOld = animName;
			animator.SetBool (animName, true);
		}

		}



		private void FixedUpdate ()
		{
			if (m_dead)	return;
			// Adjust the rigidbodies position and orientation in FixedUpdate.
		if (animName == "MoveForward") 	Move ();

		}


		private void Move ()
		{
			// Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
			Vector3 movement = transform.forward  * m_Speed * Time.deltaTime;

			// Apply this movement to the rigidbody's position.
			m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

		}




		void OnTriggerEnter(Collider col){
			if (m_dead)	return;
			if (col.gameObject.tag == "Shell") {
				CSC_Shell shell = col.GetComponent<CSC_Shell> ();
				Damage (shell.shellDamage);
			}
		}


		public void Damage(int damageAmount)
		{
			//subtract damage amount when Damage function is called
			currentHealth -= damageAmount;

			//Check if health has fallen below zero
			if (currentHealth <= 0) 
			{ //DEAD
				m_dead = true;
			animator.SetBool ("Dead"+(int)Random.Range(1,DeadMax), true);
	 
				m_MovementAudio = GetComponent<AudioSource>();
				m_MovementAudio.clip =  tankDead;
				m_MovementAudio.Play();

				transform.gameObject.tag = "Respawn"; 

				Destroy (GetComponent<CSC_UnitMovement> ());
				Destroy (GetComponent<CSC_TurretTurn> ());


            Instantiate(m_SmokeEffect, transform.position , transform.rotation);
            Instantiate(m_ExplosionParticles, transform.position + new Vector3(0, GetComponent<BoxCollider>().size.y / 2, 0), transform.rotation);
 
 
 
 
	 

				StartCoroutine (hideTnak());
			}
		}


		private IEnumerator hideTnak()
		{
			//Wait for 15 seconds
			yield return shotDuration;
			//hide tank
		//	gameObject.SetActive (false);
		}
	}
 