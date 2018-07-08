using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour {

	private int score = 0;
	private TextMeshProUGUI textmeshPro;
	void Start () {
		textmeshPro = GetComponent<TextMeshProUGUI>();
		textmeshPro.SetText(score.ToString());
	}

	void AddCrystal(int amount)
	{
		score += amount;
		textmeshPro.SetText(score.ToString());
	}

	public int GetCrystals()
	{
		return score;
	}
}
