using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Factory : MonoBehaviour {

	public GameObject dirt;
	public GameObject water;
	public Vector3 Position;
	public Vector3 Rotation;
	[Inject]
	private DiContainer _diContainer;
	
	// Update is called once per frame
	void Start () {
		
	}

	public void doIt() {
		_diContainer.InstantiatePrefab(dirt, Position, Quaternion.Euler(Rotation), null);
	}

	public void instanciate(Element Element) {
		
		_diContainer.InstantiatePrefab(Element.toInstantiate(), Position, Quaternion.Euler(Rotation), null);
	}

	public void instanciate(GameObject GameObject) {
		_diContainer.InstantiatePrefab(GameObject, Position, Quaternion.Euler(Rotation), null);
	}
}
