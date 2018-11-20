using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONScore {
	public long id;
	public long seconds;
	public long points;

}

[Serializable]
public class JSONScores {
	public List<JSONScore> scores;
}
