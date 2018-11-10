using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementJson
{
	public long id;
	public Vector3 position;
	public Quaternion rotation;
}

[System.Serializable]
public class ElementCollection
{
	public List<ElementJson> elements;
	public List<GroupJson> groups;
	public List<InteractableJson> interactables;
}

[System.Serializable]
public class GroupJson
{
	public Vector3 pA;
	public Vector3 pB;
	public ComponentJson component;
}

[System.Serializable]
public class InteractableJson
{
	public Vector3 pos;
	public int channel;
}

[System.Serializable]
public class ComponentJson
{
	public int id;
	public Vector3 position;
	public float speed;
	public int channel;
}

[System.Serializable]
public class Level {
		
	public int idlevel;
	public string namelevel;
	public ElementCollection jsonlevel;
	public string descriptionlevel;
	public string weather_saviorlevel;
	public int maxscorelevel;

}

[System.Serializable]
public class Levels
{
	public List<Level> levels;
}

[System.Serializable]
public class AddLevel
{
	public string name;
	public string json;
	public string createdAt;
	public string updatedAt;
	public string description;
	public string weather_savior;
	public int maxScore;
	public long idUser;
	
}