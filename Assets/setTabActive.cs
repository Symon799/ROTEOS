using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setTabActive : MonoBehaviour {

	// Use this for initialization
	Toggle toggle;
	void Start () {
		toggle = GetComponent<Toggle>();
		toggle.isOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
