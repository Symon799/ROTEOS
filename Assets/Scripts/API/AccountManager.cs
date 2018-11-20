using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
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

    public MenuManager menuManager;

    void Start()
    {
        if (token != null)
        {
            loginMenu.SetActive(false);
            mainMenu.SetActive(true);
            menuManager.StartMenu();
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
            StartCoroutine(updateLevels());
        }

        Debug.Log("Bye Bye from postJson");
    }

    public IEnumerator updateLevels()
    {
        Debug.Log("Welcome to Levels Getter");
        UnityWebRequest allLevelsRequest = _webRequester.Get("https://immense-lake-57494.herokuapp.com/levels", null);
        yield return new WaitUntil(() => allLevelsRequest.isDone); // To replace with personnal levels
        MetaData currentMetaData;
        if (MetaDataAction.metadataExists())
            currentMetaData = MetaDataAction.readMetaData();
        else
            currentMetaData = new MetaData();

        Debug.Log("Finish downloading...");

        if (allLevelsRequest.responseCode != 200)
            yield return null;

        Debug.Log("Start parsing... " + allLevelsRequest.downloadHandler.text);

        Levels allLevels = JsonUtility.FromJson<Levels>("{\"all\":" + allLevelsRequest.downloadHandler.text + "}");

        foreach (Level level in allLevels.all)
        {
            Debug.Log("OBJET LEVEL : " + JsonUtility.ToJson(level));
            if (!currentMetaData.levels.Exists(x => x.id == Convert.ToInt64(level.id)))
            {
                string filePath = Path.Combine(Application.persistentDataPath, "Levels/" + level.name);
                File.WriteAllText(filePath, JsonUtility.ToJson(level.json));
                currentMetaData.levels.Add(MetaDataAction.LevelToMetaDataLevel(level));
            }
        }

        Debug.Log("Current Meta Data... " + JsonUtility.ToJson(currentMetaData));

        yield return (MetaDataAction.modifyMetaData(currentMetaData));

        menuManager.StartMenu();
        

    }

    public IEnumerator updateScore()
    {
        Debug.Log("Welcome to get subscribed scores");
        yield return null;
    }

}

public class ConnectionRequest
{
    public string token;
    public bool status;
    public string error;

}