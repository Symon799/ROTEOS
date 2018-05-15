using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player")
        	Debug.Log("FINISHED !");
    }
}
