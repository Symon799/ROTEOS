using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Finish : MonoBehaviour {

    public GameObject characterPrefab;
	public GameObject SuccessImage;
	public GameObject ScoreText;

	private GameObject character;
	private Transform startPos;
	private NavMeshAgent agent;

	void Awake()
	{
		startPos = GameObject.FindGameObjectWithTag("Start").transform;
		character = Instantiate(characterPrefab, startPos);
		character.transform.position = startPos.position;
	}
	
	void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player")
		{
        	Debug.Log("FINISHED !");
			character.GetComponent<NavMeshAgent>().ResetPath();
			SuccessImage.SetActive(true);
			ScoreText.SetActive(false);		}
	}
}
