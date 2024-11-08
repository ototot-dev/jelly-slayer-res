using UnityEngine;
using System.Collections;
//This script executes commands to change character animations
[RequireComponent (typeof (Animator))]
public class Omol_Actions : MonoBehaviour {

	private Animator animator;
	void Awake () {
		animator = GetComponent<Animator> ();
    }


	public void Idle()
	{
		animator.SetBool ("OmolIdle", true);
	}

	public void Cry()
	{
		animator.SetBool ("OmolCry", true);
	}

	public void GoForwad()
	{
		animator.SetBool ("OmolGoForwad", true);
	}

	public void GoBack()
	{
		animator.SetBool ("OmolGoBack", true);
	}

	public void TurnLeft()
	{
		animator.SetBool ("OmolTurnLeft", true);
	}

	public void TurnRight()
	{
		animator.SetBool ("OmolTurnRight", true);
	}

	public void StrafeLeft()
	{
		animator.SetBool ("OmolStrafeLeft", true);
	}

	public void StrafeRight()
	{
		animator.SetBool ("OmolStrafeRight", true);
	}

	public void Run()
	{
		animator.SetBool ("OmolRun", true);
	}

	public void Sneak()
	{
		animator.SetBool ("OmolSneak", true);
	}

	public void Jump()
	{
		animator.SetBool ("OmolJump", true);
	}

	public void Attack1()
	{
		animator.SetBool ("OmolAttack1", true);
	}

	public void Attack2()
	{
		animator.SetBool ("OmolAttack2", true);
	}

	public void Attack3()
	{
		animator.SetBool ("OmolAttack3", true);
	}

	public void Attack4()
	{
		animator.SetBool ("OmolAttack4", true);
	}

	public void DamageFront1()
	{
		animator.SetBool ("OmolDmgFront1", true);
	}

	public void DamageFront2()
	{
		animator.SetBool ("OmolDmgFront2", true);
	}

	public void DamageLeft()
	{
		animator.SetBool ("OmolDmgLeft", true);
	}

	public void DamageRight()
	{
		animator.SetBool ("OmolDmgRight", true);
	}

	public void DamageBack()
	{
		animator.SetBool ("OmolDmgBack", true);
	}

	public void Dead1()
	{
		animator.SetBool ("OmolDead1", true);
	}

	public void Dead2()
	{
		animator.SetBool ("OmolDead2", true);
	}

	public void Dead3()
	{
		animator.SetBool ("OmolDead3", true);
	}

	public void Dead4()
	{
		animator.SetBool ("OmolDead4", true);
	}

}
 