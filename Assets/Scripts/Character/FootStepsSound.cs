using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FootStepsSound : MonoBehaviour {


    private Movement playerMovement;

    void Start()
    {
        playerMovement = gameObject.GetComponent<Movement>();
    }
    public void Step()
    {
        Debug.Log("Play Step...");
        Debug.Log(playerMovement);
        playerMovement.CurrentCube.gameObject.GetComponent<PlayStepSound>().StepSound();
    }
}
