using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfinder {

	void StartFindingPath(Vector3 start, Vector3 end);
	IEnumerator FindPath (Vector3 startPos, Vector3 targetPos);
	int GetDistance (Node from, Node to);
	

	// Grid functions
	void AddToGrid(Node toAdd);

	void RemoveGrid(Node toRm);
	List<Node> GetGrid();
	List<Node> GetNeighbors(Node current, float distance);
	Node NodeFromWorldPosition(Vector3 position);
}


