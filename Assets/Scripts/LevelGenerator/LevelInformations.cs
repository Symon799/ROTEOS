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
	public int x;
	public int y;
	public int z;
}

[Serializable]
public class rotation
{
	public int x;
	public int y;
	public int z;
	public int w;
}

[Serializable]
public class interactable
{
	public position pos;
	public int channel;
}
