using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Node : MonoBehaviour, IHeapItem<Node> {

	public bool cubeWalkable = true;

	public Vector3 worldPosition {
		get {
			return gameObject.transform.position;
		}
	}

	public bool walkable {
		get {
			
			foreach (var colliding in Physics.OverlapSphere(this.transform.position, 0.0001f)) {
				if (colliding.bounds.Contains(this.transform.position) && colliding.gameObject.tag != "Player" && colliding.gameObject.tag != "Collectable")
					return false;
			}
			return true && cubeWalkable;
		}
	}

	public int gCost;
	public int hCost;

	public Node parentNode;
	public GameObject cube;
	public float maxDistanceBetweenNode = 2.1f;

	[Inject]
    protected IPathfinder pathfinder;

	int heapIndex;

	void OnEnable()
	{
		pathfinder.AddToGrid(this);
	}



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

	public float getFacing() {

		RaycastHit hit;
		int layer_mask = LayerMask.GetMask("Ground");
		if (Physics.Raycast(this.worldPosition, Vector3.down, out hit, layer_mask)) 
		{
			if (hit.transform.gameObject == this.cube) { return 1; }
		}
		if (Physics.Raycast(this.worldPosition, Vector3.up, out hit, layer_mask)) 
		{
			if (hit.transform.gameObject == this.cube) { return 2; }
		}
		if (Physics.Raycast(this.worldPosition, Vector3.back, out hit, layer_mask)) 
		{
			if (hit.transform.gameObject == this.cube) { return 3; }
		}
		if (Physics.Raycast(this.worldPosition, Vector3.forward, out hit, layer_mask)) 
		{
			if (hit.transform.gameObject == this.cube) { return 4; }
		}
		if (Physics.Raycast(this.worldPosition, Vector3.right, out hit, layer_mask)) 
		{
			if (hit.transform.gameObject == this.cube) { return 5; }
		}
		if (Physics.Raycast(this.worldPosition, Vector3.left, out hit, layer_mask)) 
		{
			if (hit.transform.gameObject == this.cube) { return 6; }
		}
		return 0;
	}
}
