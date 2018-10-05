using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element
{

    public GameObject Basic;

    public Vector3 Position;

    public Quaternion Rotation;

    public Vector3 Scale;

    public List<IScript> Scripts;

    public Element(GameObject basic)
    {
        Basic = basic;
    }

    public GameObject toInstantiate()
    {
        GameObject result = this.Basic;
        //result.transform.position = this.Position;
        //result.transform.rotation = this.Rotation;
        //result.transform.localScale = this.Scale;
        if (Scripts != null)
        {
            foreach (IScript script in this.Scripts)
            {
                script.AddComponent(result);
            }
        }
        return result;
    }

	public void AddScript(IScript script)
	{
		if (Scripts == null)
			Scripts = new List<IScript>();
		Scripts.Add(script);
	}
}
