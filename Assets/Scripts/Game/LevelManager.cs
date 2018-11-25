using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject]
    private IMeteoManager _meteoManager;

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
            ScoreUI.SetActive(false);
        }
    }

	public bool GetStateWinning()
	{
		return hasWon;
	}


}
