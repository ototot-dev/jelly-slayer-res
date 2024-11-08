using UnityEngine;
using System.Collections;
//This script executes commands to change character animations
[RequireComponent (typeof (Animator))]
public class Rob_Actions : MonoBehaviour {

 
  

    private Animator animator;
 
    void Awake () {
		animator = GetComponent<Animator> ();
 
   
    }



    public void CrouchIdle()
    {
        animator.SetTrigger("CrouchIdle");
    }
    public void CrouchIdleAim()
    {
        animator.SetTrigger("CrouchIdleAim");
    }
    public void CrouchMove()
    {
        animator.SetBool("CrouchMove", true);
    }

    public void Idle1()
    {
        animator.SetTrigger("Idle1");
    }
    public void IdleAim()
    {
        animator.SetTrigger("IdleAim");
    }
    public void Idle2()
    {
        animator.SetBool("Idle2", true);
    }
    public void Idle3()
	{
		animator.SetBool ("Idle3", true);
	}
    public void Kick()
	{
		animator.SetBool ("Kick", true);
	}


	public void Walk()
	{
		animator.SetBool ("Walk", true);
	}
	public void WalkBack()
	{
		animator.SetBool ("WalkBack", true);
	}

    public void Run()
    {
        animator.SetBool("Run", true);
    }
    public void RunAim()
    {
        animator.SetBool("RunAim", true);
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
	public void Hit1()
	{
		animator.SetBool ("Hit1", true);
	}
	public void Hit2()
	{
		animator.SetBool ("Hit2", true);
	}
	public void Hit3()
	{
		animator.SetBool ("Hit3", true);
	}
	public void Hit4()
	{
		animator.SetBool ("Hit4", true);
	}
	public void Death1()
	{
		animator.SetBool ("Death1", true);

    }
	public void Death2()
	{
		animator.SetBool ("Death2", true);
    }
	public void Death3()
	{
		animator.SetBool ("Death3", true);
    }
	public void Death4()
	{
		animator.SetBool ("Death4", true);
    }
	public void ArmIdle()
	{
		animator.SetTrigger ("ArmIdle");
	}
    public void ArmAim()
    {
        animator.SetBool("ArmAim", true);
    }

    public void ArmShot()
    {
        animator.SetBool("ArmShot", true);
    }








}
