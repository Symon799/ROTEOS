using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : IPathfinder
{

    private List<Node> _grid;

    public float maxDistanceBetweenNode = 2.1f;

    public List<Node> Grid
    {
        get
        {
            if (_grid == null)
            {
                _grid = new List<Node>();
            }
            return _grid;
        }
        set
        {
            _grid = value;
        }
    }

    public void AddToGrid(Node toAdd)
    {
        Grid.Add(toAdd);
    }

    public void RemoveGrid(Node toRm)
    {
        Grid.Remove(toRm);
    }

    public List<Node> GetGrid()
    {
        return Grid;
    }

    public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Debug.Log("FINDING PATH");
        Node[] waypoints = new Node[0];
        bool wasSuccessful = false;
        Node startNode = NodeFromWorldPosition(startPos);
        Node targetNode = NodeFromWorldPosition(targetPos);

        Debug.Log("START : " + startNode.walkable + " | TARGET : " + targetNode.walkable);
        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(Grid.Count);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Debug.Log(openSet.Count);
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    Debug.Log("SUCCESSFUL");
                    wasSuccessful = true;
                    break;
                }

                foreach (Node neighbor in GetNeighbors(currentNode, maxDistanceBetweenNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                        continue;

                    int newMoveCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                    if (newMoveCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMoveCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parentNode = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }

                    }
                }

            }
        }
        else
        {
            Debug.Log("Position (" + startNode.worldPosition + ") is not walkable !");
        }

        yield return null;
        if (wasSuccessful)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        PathRequestManager.instance.FinishedProcessingPath(waypoints, wasSuccessful);

    }

    public int GetDistance(Node from, Node to)
    {
        return Convert.ToInt32(Vector3.Distance(from.worldPosition, to.worldPosition));
    }

    public void StartFindingPath(Vector3 start, Vector3 end)
    {
        Debug.Log("START FINDING PATH");
        PathRequestManager.instance.StartCoroutine(FindPath(start, end));
    }

    Node[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Insert(0, endNode);
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Insert(0, currentNode.parentNode);
            currentNode = currentNode.parentNode;
        }

        return path.ToArray();
    }

    void ResetNodes()
    {
        foreach (var node in Grid)
        {
            node.Reset();
        }
    }


    // Grid function
    public List<Node> GetNeighbors(Node current, float distance)
    {
        List<Node> neighbours = new List<Node>();
        foreach (var node in Grid)
        {
            if (Vector3.Distance(current.worldPosition, node.worldPosition) <= Math.Max(node.maxDistanceBetweenNode, current.maxDistanceBetweenNode) 
                && node.walkable 
                && node.cube != current.cube
                && current.getFacing() == node.getFacing())
            {
                neighbours.Add(node);
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPosition(Vector3 position)
    {
        Node nearest = null;
        float? nearestDistance = null;
        foreach (var node in Grid)
        {
            float tmp = Vector3.Distance(position, node.worldPosition);
            if (nearestDistance == null || Vector3.Distance(position, node.worldPosition) < nearestDistance)
            {
                nearest = node;
                nearestDistance = tmp;
            }
        }
        return nearest;
    }
}
