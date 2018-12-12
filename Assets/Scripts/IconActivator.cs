using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconActivator : MonoBehaviour {

	public GameObject rainObject;
	public GameObject snowObject;
	public bool rain;
	public bool snow;


	// Use this for initialization
	void Start ()
	{
		rainObject.SetActive(false);
		snowObject.SetActive(false);
	}

	public void toUpdate()
	{
		if (rain && !snow)
		{
			rainObject.SetActive(true);
			snowObject.SetActive(false);

		}
		else if (snow && !rain)
		{
			snowObject.SetActive(false);
			snowObject.SetActive(true);
		}
		else if (snow && rain)
		{
			rainObject.SetActive(true);
			snowObject.SetActive(true);
			snowObject.transform.position = new Vector3(snowObject.transform.position.x - 6.3f, snowObject.transform.position.y, snowObject.transform.position.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
