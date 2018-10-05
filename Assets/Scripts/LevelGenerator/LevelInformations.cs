using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonLevel
{
	public List<element> elements;
	public List<interactable> interactables;
}

[Serializable]
public class element
{
	public int id;
	public position position;
	public rotation rotation;
}

[Serializable]
public class position
{
	public float x;
	public float y;
	public float z;
}

[Serializable]
public class rotation
{
	public float x;
	public float y;
	public float z;
	public float w;
}

[Serializable]
public class interactable
{
	public position pos;
	public int channel;
}
