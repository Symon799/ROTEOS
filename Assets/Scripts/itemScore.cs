using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemScore : MonoBehaviour {

private GameObject scoreObject;
	void Start () {
		scoreObject = GameObject.FindGameObjectWithTag("Score");
	}

	void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("Player"))
		{
			scoreObject.SendMessage("AddCrystal", 100);
			Destroy(gameObject);
		}
	}
}
