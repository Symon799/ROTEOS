using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : EventClient {
	private bool _isInitial = false;

	private bool _isMoving = false;

	private Vector3 _basePosition;
	public Vector3 MoveToPosition;
	public GameObject navmeshObject;
	public float Speed = 1;

	void Awake() {
		_basePosition = this.transform.localPosition;
		AddToEvents(move);
	}
	
	void Update() {
		if (_isMoving) {
			if (isArrived())
			{
				navmeshObject.GetComponent<NavMeshSurface>().BuildNavMesh();
				_isMoving = false;
			}
			else
				moveObject();
		}
	}

	public void move() {
		_isMoving = true;
		_isInitial = !_isInitial;
	}

	private void moveObject() {
		if (_isInitial) {
			this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, MoveToPosition, Speed * Time.deltaTime);
		} else {
			this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, _basePosition, Speed * Time.deltaTime);
		}
		
	}

	private bool isArrived() {
		if (_isInitial) {
			return this.transform.localPosition == MoveToPosition;
		} else {
			return this.transform.localPosition == _basePosition;
		}
		
	}
}
