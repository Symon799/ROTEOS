using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level {

	public string name;
	public JsonLevel json;
	public string description;
	public string weather_savior;
	public int maxScore;
	public string idUser;
	public string id;
	public string createdAt;
	public string updatedAt;
}

[Serializable]
public class Levels {

	public List<Level> all;
}

[System.Serializable]
public class LevelForEd {
		
	public int idlevel;
	public string namelevel;
	public ElementCollection jsonlevel;
	public string descriptionlevel;
	public string weather_saviorlevel;
	public int maxscorelevel;

}

[System.Serializable]
public class LevelsForEd
{
	public List<LevelForEd> levels;
}
