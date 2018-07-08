using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Movement : MonoBehaviour
{

    [Inject]
    private IPathfinder _pathfinder;
    public List<Node> Route;
	public float Speed;
	public float movementThreshold;

    public Node CurrentNode = null;

    private Gravity characterGround;
    private Movement characterMovement;
	private Rigidbody characterRigidbody;

    void Start()
    {
        Route = new List<Node>();
        characterGround = GameObject.FindGameObjectWithTag("Player").GetComponent<Gravity>();
        characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
		characterRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("CLICK");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                PathRequestManager.RequestPath(characterGround.GetBotPosition(), hit.point, MoveRequest);
            }
        }
		if (CurrentNode != null)
		{
			if (hasArrived(movementThreshold))
			{
				Debug.Log("HAS ARRIVED");
				characterRigidbody.velocity = Vector3.zero;
				CurrentNode = null;
			}
			else
			{
				var direction = (CurrentNode.worldPosition - characterGround.GetBotPosition()).normalized * Speed;
                characterRigidbody.MovePosition(direction);
				characterRigidbody.velocity = direction;
			}
		}
		else
		{
			
			if (Route.Count > 0)
			{
				Debug.Log("Next NODE");
				CurrentNode = Route[0];
				Route.RemoveAt(0);
			}
		}
    }

    // Use this for initialization
    public void MoveRequest(Node[] nodes, bool success)
    {
        Debug.Log("ACTION");
        if (success)
        {
            if (Route == null)
                Route = new List<Node>();
            else
                Route.Clear();
            foreach (var node in nodes)
            {
                Route.Add(node);
            }
        }
        else
        {
            if (Route != null)
                Route.Clear();
        }
    }

	bool hasArrived(float movementThreshold)
	{
		Debug.Log("DISTANCE : " + Vector3.Distance(characterGround.GetBotPosition(), CurrentNode.worldPosition));
		return (Vector3.Distance(characterGround.GetBotPosition(), CurrentNode.worldPosition) <= movementThreshold);
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
            if (Route != null)
            {

                Gizmos.color = Color.red;
                foreach (var node in Route)
                {
                    Gizmos.DrawSphere(node.worldPosition, 0.2f);
                    foreach (var nodeNeigbor in _pathfinder.GetNeighbors(node, 2.1f))
                    {
                        if (Route.Contains(nodeNeigbor))
                            Gizmos.DrawLine(node.worldPosition, nodeNeigbor.worldPosition);
                    }
                }
            }

        }
    }
}
