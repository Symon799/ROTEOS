using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PathRequestManager : MonoBehaviour
{

    [Inject]
    private IPathfinder _pathfinder;

    #region TESTING
    public Vector3 from;
    public Vector3 to;
    private List<Node> lastWaypoints = null;

    private Gravity characterGround;
    private Movement characterMovement;

    void Start()
    {

    }

    void Update()
    {
    }

    void Awake()
    {
        instance = this;
    }

    public void testPathfinding()
    {
        Debug.Log("testPathfinding");
        characterGround = GameObject.FindGameObjectWithTag("Player").GetComponent<Gravity>();
        RequestPath(characterGround.GetBotPosition(), to, testAction);
    }

    void testAction(Node[] nodes, bool success)
    {
        Debug.Log("ACTION");
        if (success)
        {
            if (lastWaypoints == null)
                lastWaypoints = new List<Node>();
            else
                lastWaypoints.Clear();
            foreach (var node in nodes)
            {
                lastWaypoints.Add(node);
            }
        }
        else
        {
            if (lastWaypoints != null)
                lastWaypoints.Clear();
        }
    }
    #endregion

    #region VARIABLES
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    bool isProcessingPath = false;
    #endregion

    public static PathRequestManager instance;

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Node[], bool> callback;

        public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Node[], bool> _callback)
        {
            pathStart = _pathStart;
            pathEnd = _pathEnd;
            callback = _callback;
        }
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Node[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            _pathfinder.StartFindingPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Node[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    public bool Pathfinder()
    {
        return _pathfinder != null;
    }

    public void resetPathfinder()
    {
        _pathfinder.resetGrid();
        lastWaypoints = null;
    }

    void OnDrawGizmos()
    {
        if (_pathfinder != null)
        {

            foreach (var node in _pathfinder.GetGrid())
            {
                if (node.walkable)
                    Gizmos.color = Color.white;
                else
                    Gizmos.color = Color.black;
                Gizmos.DrawSphere(node.worldPosition, 0.2f);
                foreach (var nodeNeigbor in _pathfinder.GetNeighbors(node, 2.1f))
                {
                    Gizmos.DrawLine(node.worldPosition, nodeNeigbor.worldPosition);
                }

            }
            if (lastWaypoints != null)
            {

                Gizmos.color = Color.red;
                foreach (var node in lastWaypoints)
                {
                    Gizmos.DrawSphere(node.worldPosition, 0.2f);
                    foreach (var nodeNeigbor in _pathfinder.GetNeighbors(node, 2.1f))
                    {
                        if (lastWaypoints.Contains(nodeNeigbor))
                            Gizmos.DrawLine(node.worldPosition, nodeNeigbor.worldPosition);
                    }
                }
            }

        }
    }
}
