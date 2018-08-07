using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Finish : MonoBehaviour
{

    public GameObject SuccessImage;
    public GameObject ScoreText;
    private GameObject scoreObject;

    private bool hasWon = false;

    void Awake()
    {
        scoreObject = GameObject.FindGameObjectWithTag("Score");
    }

    void Update()
    {
        if (!hasWon)
        {
            foreach (Transform child in transform)
            {
                if (child.tag == "Player")
                {
                    Debug.Log("FINISHED !");
					hasWon = true;
                    SuccessImage.SetActive(true);
                    SuccessImage.GetComponentInChildren<TextMeshProUGUI>().SetText(scoreObject.GetComponent<ScoreHandler>().GetCrystals().ToString());
                    ScoreText.SetActive(false);
                }
            }
        }
    }
}
