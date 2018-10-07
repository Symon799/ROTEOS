using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MetaData
{
	public List<world> worlds;

}

[Serializable]
public class world
{
	public int id;
	public string name;
	public List<level> levels;
}

[Serializable]
public class level
{
	public string id;
	public bool rain;
	public bool hot;
	public bool cold;
}
