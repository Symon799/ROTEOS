using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour {

    public GameObject prefabParticles;
    public GameObject waterParticles;

    private  GameObject IntanceParticle;
    private bool isMoving = false;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (agent.velocity == Vector3.zero && isMoving == true)
        {
            Destroy(IntanceParticle);
            isMoving = false;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButtonDown("Fire1"))
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("Water"))
                {
                    Vector3 partPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    Instantiate(waterParticles, partPos, waterParticles.transform.rotation);
                }
                else if (!hit.collider.CompareTag("NonWalkable") && !hit.collider.CompareTag("Interact"))
                {
                    if (IntanceParticle)
                        Destroy(IntanceParticle);

                    Vector3 partPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    IntanceParticle = Instantiate(prefabParticles, partPos, hit.transform.rotation);
                    

                    agent.SetDestination(hit.transform.position);
                    isMoving = true;
                    agent.velocity = new Vector3(0.01f, 0.01f, 0.01f); //workaround for the new navMeshSystem bugged when rotated
                }
            }

    }
}
