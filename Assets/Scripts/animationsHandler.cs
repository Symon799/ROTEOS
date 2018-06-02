using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class animationsHandler : MonoBehaviour {
    NavMeshAgent agent;

	Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		animator.SetBool("walking", true);
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (agent.velocity.magnitude == 0)
			animator.SetBool("walking", false);
		else
			animator.SetBool("walking", true);
		
	}
}
