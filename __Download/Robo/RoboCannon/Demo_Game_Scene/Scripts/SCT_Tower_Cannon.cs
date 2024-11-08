using UnityEngine;
using System.Collections;

public class SCT_Tower_Cannon : MonoBehaviour
	{
		//for  AI
		public bool  AI;					//  AI ON/OFF
 
 
		public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
		public int currentHealth = 50;  			//The tank's current health point total
    
		public AudioClip tankDead;   
 
 
	 
 
 
		private Animator animator;
 
 
		private WaitForSeconds shotDuration = new WaitForSeconds(15f);    // WaitForSeconds hide object 
		private ParticleSystem m_ExplosionParticles;         //  the particles that will play on explosion.
		string animDo ; //the animation used now
	 
		



		public Transform Turret;
		public Transform Barrel;
		public Transform GunEnd;

		private Transform TargetForTurn;
		private Vector3  TargetForTurnOld;
		private float  TargetForTurnTimer;
		public float ConstraintAngle=180; 			//  limitation of rotation of the turret
		public float EnemyRangeFire=50;
		public Rigidbody shell;
		
		public float FireRate=0.5f;					// Number in seconds which controls how often the player can fire
		public float speedShell=80f;
 

		private float nextFire;                     // Float to store the time the player will be allowed to fire again, after firing
		private float lastY;                    
		private Vector3 pos_barrel;

 
		private bool m_dead; 
		private bool EnemyFire;

		private	Quaternion target;
		public ParticleSystem m_smokeBarrel;       
		public ParticleSystem m_smokeDead;       
		public ParticleSystem m_tankExplosion;       
		public AudioSource m_AudioSource;   
		public AudioClip soundFire;   

 
	 
 

		private void Awake ()
		{
			animator = GetComponentInChildren<Animator> ();
			pos_barrel = Barrel.transform.localPosition;
 
 

		}


		private void OnEnable ()
		{
 
		}


		private void OnDisable ()
		{
 
		}


		private void Start ()
		{
		{
			TargetForTurn = transform;
			if (!AI) {

				TargetForTurn = GameObject.Find ("TargetMouse").transform;
			}
			else {
				if (gameObject.tag == "Enemy") 	TargetForTurn = GameObject.FindGameObjectWithTag ("Player").transform;
				else 	TargetForTurn = GameObject.FindGameObjectWithTag ("Enemy").transform;
			}
 
		}
 
 
 
 
		}


		private void Update ()
		{
		if (m_dead)
				return;

		if(!AI && Input.GetButtonDown("Fire1")) EnemyFire = true;

		if (EnemyFire &&   nextFire<=0) {
			EnemyFire = false;
			nextFire =  FireRate;
			fire();
		 	m_smokeBarrel.Play();
		 	m_AudioSource.PlayOneShot(soundFire);
		 
			 
		}
		if (nextFire > 0) {
			nextFire -= 0.01f;
			Barrel.transform.localPosition = new Vector3 (Barrel.transform.localPosition.x,pos_barrel.y+nextFire/2,Barrel.transform.localPosition.z  );
		} 

 
 
 

		if (animDo != "Idle" ) {
				animDo = "Idle";
				animator.SetBool ("Idle" , true);
		
			}

		var dist = TargetForTurnOld - TargetForTurn.position;
		if (dist.sqrMagnitude >0.01 )  	TargetForTurnTimer = 0;
		else TargetForTurnTimer +=1;
		 
		TargetForTurnOld = TargetForTurn.position;
		}

 
 

 
 

	//Collider col = Physics.OverlapBox(enemyCheck.position, 0.6f, LayerEnemy);
		void OnTriggerEnter(Collider col){
		if (m_dead)	return;
			if (col.gameObject.tag == "Shell") {
				RC_Shell shell = col.GetComponent<RC_Shell> ();
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
				animator.SetBool ("Dead", true);
				animDo = "Dead";
				m_AudioSource.PlayOneShot(tankDead);
 
				transform.gameObject.tag = "destroyed"; 
				GetComponent<BoxCollider> ().enabled = false;
 

				m_smokeDead .Play ();
				m_tankExplosion .Play ();
	 

				StartCoroutine (hideTnak());
			}
		}

	void LateUpdate () {
		if (m_dead)	return;
		//////////////// for Enemy AI //////////////// begin
		if (AI) {
			if (TargetForTurn.gameObject.tag == "destroyed")
				return;

			var heading = Turret.transform.position - TargetForTurn.position;
			if (heading.sqrMagnitude < EnemyRangeFire ) { //if the enemy tank is far move otherwise stand
				EnemyFire = true;
			}
		}
		//////////////// for Enemy AI //////////////// end
		//turn head for mouse

 
			if (TargetForTurn)
			if (TargetForTurnTimer < 300) {
				Vector3 targetDir = TargetForTurn.position - Turret.transform.position;
				Vector3 newDir = Vector3.RotateTowards (Turret.transform.forward, targetDir, 1, 0.0F);

				target = Quaternion.LookRotation (newDir);
				float parentY=target.eulerAngles.y- transform.eulerAngles.y ;
	 
				if (parentY < 0)  parentY = -parentY;
			 	if(parentY>180) parentY-=360;

				if (parentY > -ConstraintAngle && parentY < ConstraintAngle) {
					Turret.transform.rotation = Quaternion.Euler (-90, target.eulerAngles.y, 0);
					lastY = Turret.eulerAngles.y;

				} else {
					Turret.transform.rotation = Quaternion.Euler (-90, lastY, 0);
				if(AI) nextFire =  0.02f;
				}
 
			} else if (TargetForTurnTimer < 400) {
			Turret.transform.rotation = Quaternion.RotateTowards (Turret.transform.rotation, Quaternion.Euler (-90, transform.eulerAngles.y, 0), 4f);
	
			}
 
	}

	void  fire() //shot
	{
 
	 
		Rigidbody shellInstance = Instantiate (shell, GunEnd.position, Turret.rotation) as Rigidbody;
		shellInstance.linearVelocity = speedShell *-Turret.transform.up; 


	}

	private IEnumerator hideTnak()
	{
			//Wait for 15 seconds
			yield return shotDuration;
			//hide tank
		//	gameObject.SetActive (false);
	}
}
 