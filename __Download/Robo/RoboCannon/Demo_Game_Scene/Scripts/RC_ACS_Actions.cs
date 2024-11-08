using UnityEngine;
using System.Collections;
//This script executes commands to change character animations
[RequireComponent (typeof (Animator))]
public class RC_ACS_Actions : MonoBehaviour {

	private Animator animator;
	void Awake () {
		animator = GetComponent<Animator> ();
    }
 
	public void Dead1()
	{
		animator.SetBool ("ACS_Dead1", true);
	}
	public void Dead2()
	{
		animator.SetBool ("ACS_Dead2", true);
	}
	public void Dead3()
	{
		animator.SetBool ("ACS_Dead3", true);
	}
	public void Dead4()
	{
		animator.SetBool ("ACS_Dead4", true);
	}
	public void StrafeLeft()
	{
		animator.SetBool ("ACS_StrafeLeft", true);
	}
	public void StrafeRight()
	{
		animator.SetBool ("ACS_StrafeRight", true);
	}
	public void Idle()
	{
		animator.SetBool ("ACS_Idle", true);
	}
	public void Attack()
	{
		animator.SetBool ("ACS_Attack", true);
	}
	public void TernLeft()
	{
		animator.SetBool ("ACS_TernLeft", true);
	}
	public void TernRight()
	{
		animator.SetBool ("ACS_TernRight", true);
	}
	public void ChangeToWalk()
	{
		animator.SetBool ("ACS_ChangeToWalk", true);
	}
	public void ChangeToWeels()
	{
		animator.SetBool ("ACS_ChangeToWeels", true);
	}
	public void MoveWeelsForwad()
	{
		animator.SetBool ("ACS_MoveWeelsForwad", true);
	}
	public void MoveWeelsBack()
	{
		animator.SetBool ("ACS_MoveWeelsBack", true);
	}
	public void WalkForwad()
	{
		animator.SetBool ("ACS_WalkForwad", true);
	}
	public void WalkBack()
	{
		animator.SetBool ("ACS_WalkBack", true);
	}


}
