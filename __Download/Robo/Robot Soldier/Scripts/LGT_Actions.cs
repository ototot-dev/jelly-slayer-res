using UnityEngine;
using System.Collections;
//This script executes commands to change character animations
[RequireComponent (typeof (Animator))]
public class LGT_Actions : MonoBehaviour {

 
	public GameObject[] weapArray;

	private Animator animator,animatorWeap;
 
	void Awake () {
		animator = GetComponent<Animator> ();

 
 

 
    }
 
	public void Fire()
	{
		foreach (GameObject weap in weapArray) {
			animatorWeap = weap.GetComponent <Animator> ();
			animatorWeap.SetBool ("Fire", true);
		 
		}
	}
 
 

	public void Dead1()
	{
		animator.SetBool ("Dead1", true);
	}
	public void Dead2()
	{
		animator.SetBool ("Dead2", true);
	}
	public void Dead3()
	{
		animator.SetBool ("Dead3", true);
	}
	public void Dead4()
	{
		animator.SetBool ("Dead4", true);
	}


	public void MoveForward()
	{
		animator.SetBool ("MoveForward", true);
	}
	public void MoveLeft()
	{
		animator.SetBool ("MoveLeft", true);
	}
	public void MoveRight()
	{
		animator.SetBool ("MoveRight", true);
	}
	public void MoveBack()
	{
		animator.SetBool ("MoveBack", true);
	}
	public void Idle()
	{
		animator.SetBool ("Idle", true);
	}
 

}
