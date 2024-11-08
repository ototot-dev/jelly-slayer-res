using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSC_weaponFire : MonoBehaviour {
	public GameObject Shell;
	public Transform Gun_End;
    
	public float shellSpeed=5000;
	public float randomDir=20;

	public ParticleSystem m_smokeBarrel;    //Particle effect shot  
    private AudioSource m_AudioSource;  	//Sound effect shot 
	public AudioClip soundFire;
  //  string _tag;
    // Use this for initialization
    void Start()
    {
       m_AudioSource = gameObject.GetComponentInParent<AudioSource>();
     //  _tag = transform.root.gameObject.tag;
 
    }

    void  fire() //shot
    {


        var gameOb = (GameObject) Instantiate( Shell,  Gun_End.transform.position,Gun_End.transform.rotation) ; 
		Vector3 dir = new Vector3(Random.Range(-randomDir, randomDir), Random.Range(-randomDir, randomDir), Random.Range(-randomDir,randomDir)) ;
		dir+=Gun_End.forward*shellSpeed;
     //   gameOb.layer =  LayerMask.NameToLayer(_tag);

        gameOb.GetComponent<Rigidbody>().AddForce( dir);
    
        if (m_smokeBarrel) m_smokeBarrel.Play();
		if (m_AudioSource) {
            m_AudioSource.PlayOneShot(soundFire);
		}
	}

}
  