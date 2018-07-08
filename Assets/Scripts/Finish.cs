using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Finish : MonoBehaviour {

    public GameObject characterPrefab;
	public GameObject SuccessImage;
	public GameObject ScoreText;
	private GameObject scoreObject;

	private GameObject character;
	private Transform startPos;
	private NavMeshAgent agent;

	void Awake()
	{
		startPos = GameObject.FindGameObjectWithTag("Start").transform;
		character = Instantiate(characterPrefab, startPos);
		character.transform.position = startPos.position;
		scoreObject = GameObject.FindGameObjectWithTag("Score");
	}
	
	void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player")
		{
        	Debug.Log("FINISHED !");
			character.GetComponent<NavMeshAgent>().ResetPath();
			SuccessImage.SetActive(true);
			SuccessImage.GetComponentInChildren<TextMeshProUGUI>().SetText(scoreObject.GetComponent<ScoreHandler>().GetCrystals().ToString());
			ScoreText.SetActive(false);		}
	}
}
