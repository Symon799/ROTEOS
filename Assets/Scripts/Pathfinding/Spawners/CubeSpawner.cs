using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CubeSpawner : Spawner {

	public override List<Node> spawnPoints() {
		List<Node> points = new List<Node>();
		points.Add(topPoint());

		return points;
	}

	private Node topPoint() {
		Vector3 size = GetComponent<Collider>().bounds.size;
		Vector3 curPos = this.transform.position;

		GameObject nodePoint = Instantiate(nodePrefab, curPos + new Vector3(0, size.y / 2, 0), Quaternion.identity);
		nodePoint.transform.parent = gameObject.transform;

		Debug.Log(nodePoint.GetComponent<Node>().gCost);
		return nodePoint.GetComponent<Node>();
	}
}
