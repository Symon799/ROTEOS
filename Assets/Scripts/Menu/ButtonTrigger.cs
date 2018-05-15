using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonTrigger : MonoBehaviour {

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayProto1()
    {
        SceneManager.LoadScene("Prototype");
    }

    public void PlayProto2()
    {
        SceneManager.LoadScene("ButtonProto");
    }

    public void SetMusic(float value)
    {
       GetComponent<AudioSource>().volume = value;
    }
}
