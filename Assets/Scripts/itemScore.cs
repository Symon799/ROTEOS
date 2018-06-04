using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemScore : MonoBehaviour {

	public GameObject electricParticles;

private GameObject scoreObject;
	void Start () {
		scoreObject = GameObject.FindGameObjectWithTag("Score");
	}

	void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("Player"))
		{
			scoreObject.SendMessage("AddCrystal", 100);
			Instantiate(electricParticles, gameObject.transform.position, electricParticles.transform.rotation);
			Destroy(gameObject, 0.5f);
		}
	}
}
