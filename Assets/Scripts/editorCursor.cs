using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editorCursor : MonoBehaviour {
	private GameObject current;

	public GameObject bloc;
	public Transform level;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
			current = Instantiate(bloc, level);

	}
}
