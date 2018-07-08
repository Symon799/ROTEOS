using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    public float thrust = 1.0f;
    public float Speed = 1.0f;
    private Rigidbody rb;

	private Vector3 botPosition;

    private float normalSwitchRange = 5.0f;
    private float personalGravity = 9.8f;
    private Vector3 personalNormal;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        personalNormal = transform.up;
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		UpdatePersonalNormal();
        //rb.AddForce(-personalGravity * rb.mass * personalNormal);
    }

    void OnTriggerStay(Collider other)
    {
		//Debug.Log("NEW PARENT");
        if (other.gameObject.tag == "Walkable")
        {
            /*transform.SetParent(other.transform, true);
			ChildOf = other.gameObject;*/
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Walkable")
        {
            /*transform.SetParent(null, true);
			ChildOf = null;*/
        }
    }

    public void UpdatePersonalNormal()
	{
		Ray ray = new Ray(this.transform.position, -this.transform.up);
		//Debug.Log("UP : " + this.transform.up);
		RaycastHit hit;
		
		Debug.DrawRay(this.transform.position, -this.transform.up, Color.cyan);
		if (Physics.Raycast(ray , out hit, float.PositiveInfinity))
        {
			Debug.DrawLine(this.transform.position, hit.point, Color.red);
			botPosition = hit.point;
			//transform.parent = hit.transform;
            personalNormal = hit.normal; //set personal normal ...
        }
	}

	public Vector3 GetBotPosition()
	{
		return botPosition;
	}
}

