using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public Material WaterMaterial;

    void Start()
    {
		mergeWater();
    }


    public void mergeWater()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Water");
		if (gameObjects.Length == 0)
			return;
        List<MeshFilter> meshFilters = new List<MeshFilter>();
		foreach (var obj in gameObjects) {
			meshFilters.Add(obj.GetComponent<MeshFilter>());
		}
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        int i = 0;
        while (i < meshFilters.Count)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			foreach (Transform child in meshFilters[i].gameObject.transform) {
				
				GameObject square = new GameObject();
				square.transform.SetParent(transform);
				child.GetComponent<Node>().cubeWalkable = false;
				child.GetComponent<Node>().cube = square.gameObject;
				child.transform.SetParent(square.transform, true);
			}
			meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, false, true, true);
		transform.GetComponent<Renderer>().material = WaterMaterial;
		var script = transform.gameObject.AddComponent<Water>();
		script.waveFrequency = 0.1f;
		script.waveHeight = 0.1f;
		var collider = transform.gameObject.AddComponent<MeshCollider>();
		collider.convex = true;
		collider.enabled = false;
        transform.gameObject.SetActive(true);
    }
}
