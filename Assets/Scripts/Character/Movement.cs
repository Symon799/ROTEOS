using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Movement : MonoBehaviour
{

    [Inject]
    private IPathfinder _pathfinder;
    public List<Node> Route;

    public float velocity = 5;
    public float turnSpeed = 10;

    public float angleThreshold = 0.1f;
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public float maxGroundAngle = 120;

    public float groundDistance = 0.5f;

    float angle
    {
        get
        {
            if (CurrentNode != null)
            {
                return Vector3.Angle(forward, CurrentNode.worldPosition - transform.position);
            }
            else return float.NaN;
        }
    }

    public Transform CurrentCube
    {
        get
        {
            Ray ray = new Ray(this.transform.position, -this.transform.up);
            RaycastHit hit;
            int layer_mask = LayerMask.GetMask("Ground");
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layer_mask))
                return hit.transform;
            else
                return null;
        }
    }

    float groundAngle;
    Vector3 forward;
    RaycastHit hitInfo;

    public float movementThreshold;

    private Vector3 botPosition;
    private Vector3 personalNormal;
    public float personalGravity = 9.8f;
    private Vector3 locScale;

    public Node CurrentNode = null;
    private Movement characterMovement;
    private Rigidbody characterRigidbody;
    private CapsuleCollider characterCollider;

    void Start()
    {
        Route = new List<Node>();
        characterMovement = GetComponent<Movement>();
        characterRigidbody = GetComponent<Rigidbody>();
        characterCollider = GetComponent<CapsuleCollider>();

        personalNormal = transform.up;
        characterRigidbody.freezeRotation = true;
        characterRigidbody.useGravity = false;
        locScale = transform.localScale;
    }

    void Update()
    {

        //transform.localScale = locScale;
        //Debug.Log(DefaultTrackableEventHandler.trackableFounded);

        /* A MODIFIER POUR LE JEU MOBILE -> NECESSAIRE POUR LE FONCTIONNEMENT AVEC VUFORIA*/
        /*if (!DefaultTrackableEventHandler.trackableFounded)
            return;*/

        if (Input.GetButtonDown("Fire1"))
        {
            //Debug.Log("CLICK");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                //Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                PathRequestManager.RequestPath(botPosition, hit.point, MoveRequest);
            }
        }
        UpdatePersonalNormal();
        CalculateForward();
        ApplyGravity();
        if (CurrentNode != null)
        {
            //Debug.DrawLine(botPosition, CurrentNode.transform.position, Color.black);
            //Debug.Log("Rotation...");
            Rotate();
            if (hasArrived(movementThreshold))
            {
                CurrentNode = null;
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
            else
            {
                characterRigidbody.velocity = Vector3.zero;
            }
        }
    }

    void Rotate()
    {
        Vector3 toPoint = CurrentNode.worldPosition - transform.position;
        float angle = Vector3.SignedAngle(Vector3.ProjectOnPlane(toPoint, transform.up), transform.forward, transform.up);

        if (Mathf.Abs(angle) <= angleThreshold)
        {   
            if ((CurrentCube.tag == "Walkable" && CurrentNode.cube.tag == "Water") ||
            (CurrentCube.tag == "Water" && CurrentNode.cube.tag == "Walkable"))
            {
                Debug.Log("G to W");
                Jump();
            } else
            {
                Move();
            }
            return;
        }
        if (Mathf.Abs(angle) > 45f)
            characterRigidbody.velocity = characterRigidbody.velocity / 100;
        Debug.Log("SIGN : " + angle);
        if (Mathf.Sign(angle) == -1)
        {
            //Debug.Log("RIGHT");
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0, Space.Self);
        }
        else
        {
            //Debug.Log("LEFT");
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0, Space.Self);
        }
    }

    void Move()
    {
        if (groundAngle >= maxGroundAngle || !isGrounded()) return;
        characterRigidbody.AddForce(forward * velocity * Time.deltaTime, ForceMode.Acceleration);
    }

    void Jump()
    {   
        //if (isGrounded())
            characterRigidbody.AddForce((personalNormal + forward) * Vector3.Distance(botPosition, CurrentNode.worldPosition) * velocity/2 * Time.deltaTime, ForceMode.Impulse);
    }

    void CalculateForward()
    {
        if (!isGrounded())
        {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(transform.right, personalNormal);
    }

    void CalculateGroundAngle()
    {
        if (!isGrounded())
        {
            groundAngle = 90;
            return;
        }
        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    bool isGrounded()
    {
        RaycastHit hit;
        return (Vector3.Distance(characterCollider.bounds.center, botPosition) <= characterCollider.height / 2 + 0.2f);
    }

    void ApplyGravity()
    {

        if (!isGrounded())
        {
            //Debug.Log("Applying gravity...");
            characterRigidbody.AddForce(-personalGravity * characterRigidbody.mass * personalNormal);

        }
    }

    // Use this for initialization
    public void MoveRequest(Node[] nodes, bool success)
    {
        //Debug.Log("ACTION");
        if (success)
        {
            if (Route == null)
                Route = new List<Node>();
            else
            {
                Route.Clear();
                CurrentNode = null;
                characterRigidbody.velocity = Vector3.zero;
            }
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

    public void UpdatePersonalNormal()
    {
        int layer_mask = LayerMask.GetMask("Ground");
        Ray ray = new Ray(this.transform.position, -this.transform.up);
        //Debug.Log("UP : " + this.transform.up);
        RaycastHit hit;

        //Debug.DrawRay(this.transform.position, -this.transform.up, Color.cyan);
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layer_mask))
        {
            botPosition = hit.point;
            personalNormal = hit.normal; //set personal normal ...
            if (isGrounded())
            {
                transform.SetParent(hit.transform, true);
            }
        }
    }

    bool hasArrived(float movementThreshold)
    {
        //Debug.Log("DISTANCE : " + Vector3.Distance(botPosition, CurrentNode.worldPosition));
        return (Vector3.Distance(botPosition, CurrentNode.worldPosition) <= movementThreshold);
    }

    void OnDrawGizmos()
    {
        if (_pathfinder != null)
        {

            /*Gizmos.color = Color.blue;
            Gizmos.DrawSphere(botPosition, 0.3f);
            Gizmos.DrawLine(transform.position, transform.position + forward * height);
            foreach (var node in _pathfinder.GetGrid())
            {
                if (node.walkable)
                    Gizmos.color = Color.white;
                else
                {
                    Gizmos.color = Color.black;
                }
                Gizmos.DrawSphere(node.worldPosition, 0.2f);
                foreach (var nodeNeigbor in _pathfinder.GetNeighbors(node, 2.1f))
                {
                    Gizmos.DrawLine(node.worldPosition, nodeNeigbor.worldPosition);
                }
            }*/
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
            if (CurrentNode != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(CurrentNode.worldPosition, 0.2f);
            }

        }
    }
}