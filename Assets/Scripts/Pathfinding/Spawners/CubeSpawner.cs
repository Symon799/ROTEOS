using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CubeSpawner : Spawner {

	public override List<Node> spawnPoints() {
		List<Node> points = new List<Node>();
		points.Add(topPoint());
		points.Add(botPoint());
		points.Add(leftPoint());
		points.Add(rightPoint());
		points.Add(forwardPoint());
		points.Add(backwardPoint());
		return points;
	}

	private Node topPoint() {
		Vector3 size = GetComponent<Collider>().bounds.size;
		Vector3 curPos = this.transform.position;

		GameObject nodePoint = Instantiate(nodePrefab, curPos + new Vector3(0, size.y / 2 + 0.1f, 0), Quaternion.identity);
		nodePoint.transform.parent = gameObject.transform;
		nodePoint.GetComponent<Node>().cube = this.gameObject;

		//Debug.Log(nodePoint.GetComponent<Node>().gCost);
		return nodePoint.GetComponent<Node>();
	}

	private Node botPoint() {
		Vector3 size = GetComponent<Collider>().bounds.size;
		Vector3 curPos = this.transform.position;

		GameObject nodePoint = Instantiate(nodePrefab, curPos + new Vector3(0, -size.y / 2 - 0.1f, 0), Quaternion.identity);
		nodePoint.transform.parent = gameObject.transform;
		nodePoint.GetComponent<Node>().cube = this.gameObject;

		//Debug.Log(nodePoint.GetComponent<Node>().gCost);
		return nodePoint.GetComponent<Node>();
	}

	private Node leftPoint() {
		Vector3 size = GetComponent<Collider>().bounds.size;
		Vector3 curPos = this.transform.position;

		GameObject nodePoint = Instantiate(nodePrefab, curPos + new Vector3(0, 0, size.z / 2 + 0.1f), Quaternion.identity);
		nodePoint.transform.parent = gameObject.transform;
		nodePoint.GetComponent<Node>().cube = this.gameObject;

		//Debug.Log(nodePoint.GetComponent<Node>().gCost);
		return nodePoint.GetComponent<Node>();
	}

	private Node rightPoint() {
		Vector3 size = GetComponent<Collider>().bounds.size;
		Vector3 curPos = this.transform.position;

		GameObject nodePoint = Instantiate(nodePrefab, curPos + new Vector3(0, 0, -size.z / 2 - 0.1f), Quaternion.identity);
		nodePoint.transform.parent = gameObject.transform;
		nodePoint.GetComponent<Node>().cube = this.gameObject;

		//Debug.Log(nodePoint.GetComponent<Node>().gCost);
		return nodePoint.GetComponent<Node>();
	}

	private Node forwardPoint() {
		Vector3 size = GetComponent<Collider>().bounds.size;
		Vector3 curPos = this.transform.position;

		GameObject nodePoint = Instantiate(nodePrefab, curPos + new Vector3(size.x / 2 + 0.1f, 0, 0), Quaternion.identity);
		nodePoint.transform.parent = gameObject.transform;
		nodePoint.GetComponent<Node>().cube = this.gameObject;

		//Debug.Log(nodePoint.GetComponent<Node>().gCost);
		return nodePoint.GetComponent<Node>();
	}

	private Node backwardPoint() {
		Vector3 size = GetComponent<Collider>().bounds.size;
		Vector3 curPos = this.transform.position;

		GameObject nodePoint = Instantiate(nodePrefab, curPos + new Vector3(-size.x / 2 - 0.1f, 0, 0), Quaternion.identity);
		nodePoint.transform.parent = gameObject.transform;
		nodePoint.GetComponent<Node>().cube = this.gameObject;

		//Debug.Log(nodePoint.GetComponent<Node>().gCost);
		return nodePoint.GetComponent<Node>();
	}

	
}
