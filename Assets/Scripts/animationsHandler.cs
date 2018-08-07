using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class animationsHandler : MonoBehaviour {
    Rigidbody rigidbody;

	Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		animator.SetBool("walking", true);
		rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log("Magnitude : " + rigidbody.velocity.magnitude);
		if (rigidbody.velocity.magnitude == 0)
		{
			animator.SetBool("walking", false);
		}
		else
		{
			animator.SetBool("walking", true);
			animator.speed = rigidbody.velocity.magnitude;
		}
		/*if (agent.velocity.magnitude == 0)
			animator.SetBool("walking", false);
		else
			animator.SetBool("walking", true);*/
		
	}
}
