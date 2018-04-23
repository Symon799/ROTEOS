using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : EventClient {
	private bool _isInitial = false;

	private bool _isMoving = false;

	private Vector3 _basePosition;
	public Vector3 MoveToPosition;
	public float Speed = 1;

	void Awake() {
		_basePosition = this.transform.position;
		AddToEvents(move);
	}
	
	void Update() {
		if (_isMoving) {
			if (isArrived()) { _isMoving = false; }
			else {
				moveObject();
			}
		}
	}

	public void move() {
		Debug.Log("MOVING PART");
		_isMoving = true;
		_isInitial = !_isInitial;
	}

	private void moveObject() {
		Debug.Log("moveObject PART");
		if (_isInitial) {
			this.transform.position = Vector3.MoveTowards(this.transform.position, MoveToPosition, Speed * Time.deltaTime);
		} else {
			this.transform.position = Vector3.MoveTowards(this.transform.position, _basePosition, Speed * Time.deltaTime);
		}
		
	}

	private bool isArrived() {
		if (_isInitial) {
			return this.transform.position == MoveToPosition;
		} else {
			return this.transform.position == _basePosition;
		}
	}
}
