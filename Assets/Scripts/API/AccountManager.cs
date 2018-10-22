using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AccountManager : MonoBehaviour
{

    [Inject]
    private IWebRequester _webRequester;

    public static string token = null;

    public InputField accountInput;
    public InputField passwordInput;
    public GameObject loginMenu;
    public GameObject mainMenu;

    void Start()
    {
        if (token != null)
        {
            loginMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void connexion()
    {
        bool success = true;
        StartCoroutine(connectJson());
    }

    [System.Serializable]
    public class user
    {
        public string username;
        public string password;
    }


    public IEnumerator connectJson()
    {
        Debug.Log("Welcome to connexion");
        string sailsUrl = "https://secure-sands-20186.herokuapp.com/connexion";

        user body = new user();
        body.username = accountInput.text;
        body.password = passwordInput.text;
        string bodyJson = JsonUtility.ToJson(body);

        yield return StartCoroutine(_webRequester.PostComplete2(sailsUrl, bodyJson));

        if (token != null)
        {
            loginMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

        Debug.Log("Bye Bye from postJson");
    }

}

public class ConnectionRequest
{
    public string token;
    public bool status;
    public string error;

}