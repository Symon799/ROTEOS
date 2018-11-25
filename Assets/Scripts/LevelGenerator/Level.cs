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
