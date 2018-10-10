using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class AgentScript : MonoBehaviour
{

    public GameObject prefabParticles;
    public GameObject waterParticles;
    public Transform parent;

    private GameObject InstanceParticle = null;
    private Movement movement;

    // Use this for initialization
    void Start()
    {
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("Water"))
                {
                    Vector3 partPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    GameObject water = Instantiate(waterParticles, partPos, waterParticles.transform.rotation);
                    water.transform.parent = parent;
                }
            }
        }

        UpdateRouteParticles();


    }

    void UpdateRouteParticles()
    {
        if (movement.Route.Count > 0 && InstanceParticle == null)
        {
            Node tmp = movement.Route.LastOrDefault();
            InstanceParticle = Instantiate(prefabParticles, tmp.worldPosition, tmp.transform.localRotation);
            InstanceParticle.transform.parent = parent;
        }
        else if (InstanceParticle != null && movement.CurrentNode == null && movement.Route.Count == 0)
        {
            Destroy(InstanceParticle);
            InstanceParticle = null;
        }
    }
}
