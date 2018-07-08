using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : IScript
{
	public Channel Channel;
	 
	public Vector3 RotateTo;

	public float Speed;
    public void AddComponent(GameObject where)
    {
        Rotate.CreateComponent(where, Channel, RotateTo, Speed);
    }
}
