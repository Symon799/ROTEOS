using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawn : EventClient
{

    public GameObject ToSpawn;
    public Vector3 ToSpawnCoordinates;
    public Vector3 ToSpawnRotation;
    private bool _hasSpawned = false;

	private DiContainer _container;

    // Use this for initialization
    void Awake()
    {
        Debug.Log("SPAWN TRIGGER");
		Debug.Log(this.channel);
        AddToEvents(spawn);
    }

    // Update is called once per frame
    public void spawn()
    {
        if (!_hasSpawned)
        {
			Debug.Log("Instantiate");
            _container.InstantiatePrefab(ToSpawn, ToSpawnCoordinates, Quaternion.Euler(ToSpawnRotation), null);
			_hasSpawned = true;
        }
    }
}
