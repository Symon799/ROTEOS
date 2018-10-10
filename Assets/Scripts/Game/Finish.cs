using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Finish : MonoBehaviour
{
    private LevelManager LevelManager;

    void Awake()
    {
        LevelManager = FindObjectOfType(typeof(LevelManager)) as LevelManager;
    }

    void Update()
    {
        if (LevelManager != null && !LevelManager.GetStateWinning())
        {
            foreach (Transform child in transform)
            {
                if (child.tag == "Player")
                {
                    LevelManager.EndGame();
                }
            }
        }
    }
}
