using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script is for the  shell
public class RC_Shell : MonoBehaviour {
	public float lifeTime = 2.0f;
	public int shellDamage = 10;
	public ParticleSystem m_ExplosionParticles;         // Reference to the particles that will play on explosion.
	public AudioSource m_ExplosionAudio;             
	void OnTriggerEnter(Collider col){
  
 

		// Play the particle system.
		m_ExplosionParticles.Play();
		// Play the explosion sound effect.
		m_ExplosionAudio.Play();
		GetComponent<Rigidbody> ().linearVelocity =Vector3.zero;
		GetComponent<Collider> ().enabled = false;
		GetComponent<Renderer> ().enabled = false;
	 	Destroy (gameObject, 2);	 

 
 
	}
	private IEnumerator Start()
	{

		yield return new WaitForSeconds(lifeTime);
		  Destroy(gameObject);
	}
}
