using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, IHeapItem<Node> {

	public Vector3 worldPosition {
		get {
			return gameObject.transform.position;
		}
	}

	public bool walkable {
		get {
			return true;
		}
	}

	public int gCost;
	public int hCost;

	public Node parentNode;

	int heapIndex;

	public int fCost {
		get {
			return gCost+hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo (Node toCompare) {
		int compare = fCost.CompareTo(toCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(toCompare.hCost);
		}
		return -compare;
	}

	public void Reset() {
		gCost = 0;
		hCost = 0;
		parentNode = null;
	}
}
