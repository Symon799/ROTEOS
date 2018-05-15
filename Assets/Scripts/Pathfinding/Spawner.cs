using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [Inject]
    protected IPathfinder pathfinder;
    protected List<Node> points;
    public GameObject nodePrefab;

    void Start()
    {
        points = spawnPoints();
        foreach (var point in points)
        {
            pathfinder.AddToGrid(point);
        }
    }

    // To implement in other classes
    public virtual List<Node> spawnPoints()
    {
        return null;
    }
}
