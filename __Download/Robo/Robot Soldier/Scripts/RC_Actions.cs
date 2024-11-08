using UnityEngine;
using System.Collections;
//This script executes commands to change character animations
[RequireComponent (typeof (Animator))]
public class RC_Actions : MonoBehaviour {


    public GameObject shell;
    public GameObject health;
    float sz;

    private Animator animator;
    RoboCannon_Movement _RoboCannon_Movement;
    void Awake () {
		animator = GetComponent<Animator> ();
        _RoboCannon_Movement = GetComponent<RoboCannon_Movement> ();
        sz=health.GetComponent<RectTransform>().localScale.x;
    }
 


	public void Idle1()
	{
		animator.SetTrigger("Idle1");
        health.GetComponent<RectTransform>().localScale = new Vector3(sz  , 1, 1);
        transform.GetComponent<RoboCannon_Movement>().enabled = true;
    }
	public void Idle2()
	{
		animator.SetBool ("Idle2", true);
	}
	public void Idle3()
	{
		animator.SetBool ("Idle3", true);
	}


	public void Move()
	{
		animator.SetBool ("Move", true);
	}
	public void MoveBack()
	{
		animator.SetBool ("MoveBack", true);
	}
	public void MoveLimp()
	{
		animator.SetBool ("MoveLimp", true);
	}
	public void Run()
	{
		animator.SetBool ("Run", true);
	}
	public void TurnLeft()
	{
		animator.SetBool ("TurnLeft", true);
	}
	public void TurnRight()
	{
		animator.SetBool ("TurnRight", true);
	}
	public void StrafeLeft()
	{
		animator.SetBool ("StrafeLeft", true);
	}
	public void StrafeRight()
	{
		animator.SetBool ("StrafeRight", true);
	}
	public void DMG1()
	{
		animator.SetBool ("DMG1", true);
	}
	public void DMG2()
	{
		animator.SetBool ("DMG2", true);
	}
	public void DMG3()
	{
		animator.SetBool ("DMG3", true);
	}
	public void DMG4()
	{
		animator.SetBool ("DMG4", true);
	}
	public void Dead1()
	{
		animator.SetBool ("Dead1", true);
        transform.GetComponent<RoboCannon_Movement>().enabled = false;

    }
	public void Dead2()
	{
		animator.SetBool ("Dead2", true);
        transform.GetComponent<RoboCannon_Movement>().enabled = false;
    }
	public void Dead3()
	{
		animator.SetBool ("Dead3", true);
        transform.GetComponent<RoboCannon_Movement>().enabled = false;
    }
	public void Dead4()
	{
		animator.SetBool ("Dead4", true);
        transform.GetComponent<RoboCannon_Movement>().enabled = false;
    }
	public void Shot()
	{
		animator.SetTrigger ("Shot");
	}
	public void BigShot()
	{
		animator.SetBool ("BigShot", true);
	}


    public void Resurrect()
    {
        _RoboCannon_Movement.Init();
        health.GetComponent<RectTransform>().localScale = new Vector3(sz, 1, 1);
        transform.GetComponent<RoboCannon_Movement>().enabled = true;
        animator.SetTrigger("Idle1");
    }
	public void GetDamage()
	{
       // var gameOb = (GameObject)Instantiate(shell, transform.position+ new Vector3(0,5,0), transform.rotation);
 
      //  gameOb.layer = LayerMask.NameToLayer("Team2");
        health.GetComponent<RectTransform>().localScale = new Vector3(health.GetComponent<RectTransform>().localScale.x - sz*0.1f, 1, 1);



    }



 

}
