using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject]
    private IMeteoManager _meteoManager;

    [Inject]
    private IWebRequester _webRequester;

    public LevelGenerator LevelGenerator;
    public GameObject LevelEndingInterface;
    public GameObject ScoreUI;
    public TextMeshProUGUI scoreText;

    public bool editorMode = false;

    // SCORE VARIABLES
    private int nbScore = 0;
    private int nbScoreObject;
    private float startingTime;

    // GAME VARIABLES

    private bool hasWon = false;


    // Use this for initialization

    void Start() {
        if (!editorMode)
            StartLevel();
    }

    public void StartLevel()
    {
        //In case of an error here, put the tag "Managers" on the gameObject containing all of the managers
        GameObject.FindGameObjectWithTag("Managers").GetComponentInChildren<PathRequestManager>().resetPathfinder();

        InitializeLevel();
        _meteoManager.applyMeteo();

    }

    public void resetLevel()
    {
        hasWon = false;
        LevelEndingInterface.SetActive(false);
        ScoreUI.SetActive(true);
        nbScore = 0;
        GameObject.FindGameObjectWithTag("Managers").GetComponentInChildren<PathRequestManager>().resetPathfinder();
    }

    void InitializeLevel()
    {
        nbScore = 0;
        startingTime = Time.fixedTime;
        LevelGenerator.InitializeGame();
    }

    public void AddCrystal(int amount)
    {
        nbScore += amount;
        if (nbScore <= 0)
            nbScore = 0;
        UpdateCrystalScore();
    }

    public void RemoveCrystal(int amount)
    {
        nbScore -= amount;
        if (nbScore <= 0)
            nbScore = 0;
        UpdateCrystalScore();
    }

    private void UpdateCrystalScore()
    {
        scoreText.SetText(nbScore.ToString());
    }

    private string GetTimeScore()
    {
        float elapsed_time = Time.fixedTime - startingTime;
        int min = (int)(elapsed_time / 60);
        int sec = (int)(elapsed_time % 60);
        return (min + ":" + sec);
    }

    public void EndGame()
    {
        if (!hasWon)
        {
            hasWon = true;
            LevelEndingInterface.SetActive(true);
			TextMeshProUGUI[] textmeshes = LevelEndingInterface.GetComponentsInChildren<TextMeshProUGUI>();
            textmeshes[0].SetText(nbScore.ToString());
			textmeshes[1].SetText(GetTimeScore());
            if (!editorMode)
            {
                JSONScore newScore = new JSONScore();
                newScore.id = LevelGenerator.levelId;
                newScore.points = nbScore;
                newScore.seconds = Convert.ToInt64(Time.fixedTime - startingTime);
                List<JSONScore> list = new List<JSONScore>();
                list.Add(newScore);
                if (JSONScoreActions.addScore(list))
                {
                    StartCoroutine(sendScores(newScore.points));
                }
                ScoreUI.SetActive(false);
            }
        }
    }

    public IEnumerator sendScores(long score)
    {
        Debug.Log("Welcome to send score");
        string sailsUrl = "https://immense-lake-57494.herokuapp.com/scores";

        Score body = new Score();
        body.user_id = Convert.ToString(AccountManager.idCurrentUser);
        body.level_id = Convert.ToString(LevelGenerator.levelId);
        body.score = score;
        body.updatedAt = null;
        body.createdAt = null;
        string bodyJson = JsonUtility.ToJson(body);

        yield return StartCoroutine(_webRequester.PostComplete(sailsUrl, bodyJson));

        Debug.Log("Bye Bye from postJson");
    }

	public bool GetStateWinning()
	{
		return hasWon;
	}


}

public class Score
{
    public string user_id;
    public string level_id;
    public long score;
    public String createdAt;
    public String updatedAt;
}
