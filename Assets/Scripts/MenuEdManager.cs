using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;
using System.IO;
public class MenuEdManager : MonoBehaviour {

    [Inject]
	private IWebRequester _webRequester;
    private Levels levelsObj;
    private bool initialized = false;

    public Transform grid;
    public GameObject levelButtonGroup;
    public GameObject menuEditor;
	public GameObject editor;
	
	// Update is called once per frame
	void Update () {
        if (AccountManager.idCurrentUser != 0 && !initialized)
        {
            initList();
            initialized = true;
        }
	}

    private void initList()
    {
        //StartCoroutine(addLevel(AccountManager.idCurrentUser));
        StartCoroutine(getLevels(AccountManager.idCurrentUser));
    }

    //working
	public IEnumerator getLevels(long idUser)
    {
        Debug.Log("Getting levels for user " + idUser + "...");
        string sailsUrl = "https://secure-sands-20186.herokuapp.com/getalllevelbyiduserwithjson";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters.Add("iduser", idUser.ToString());


        UnityWebRequest resultRequest =_webRequester.Get(sailsUrl, parameters);

        yield return new WaitUntil(() => resultRequest.isDone);
        Debug.Log(resultRequest.responseCode + " Result : " + resultRequest.downloadHandler.text);

        levelsObj = JsonUtility.FromJson<Levels>("{\"levels\":" + resultRequest.downloadHandler.text + "}");
        UpdateList();
    }

    public void UpdateList()
    {
        foreach (Transform child in grid.transform)
            GameObject.Destroy(child.gameObject);

        foreach(Level lvl in levelsObj.levels)
        {
            GameObject button = Instantiate(levelButtonGroup, grid);
            button.GetComponent<ButtonUpdater>().setValues(lvl, menuEditor, editor);
        }
    }

    //working
    public IEnumerator addLevel(long idUser)
    {
        Debug.Log("Adding level to user " + idUser + "...");
        string sailsUrl = "https://immense-lake-57494.herokuapp.com/levels";

        AddLevel body = new AddLevel();
        body.name = "Second Level";
        string content = File.ReadAllText(Application.dataPath + "/Levels/level.json");
        body.json = content;
        body.createdAt = null;
        body.updatedAt = null;
        body.description = "This level is very simple and is simply a test";
        body.weather_savior = "Clear";
        body.maxScore = 1000;
        body.idUser = idUser;

        string bodyJson = JsonUtility.ToJson(body);

        yield return StartCoroutine(_webRequester.PostCompleteConnection(sailsUrl, bodyJson));

        Debug.Log("Level Added");
    }
    public IEnumerator deleteLevel(long idLevel)
    {
        Debug.Log("Deleting level " + idLevel + "...");
        string sailsUrl = "https://immense-lake-57494.herokuapp.com/levels";

        UnityWebRequest resultRequest =_webRequester.Delete(sailsUrl, idLevel);

        yield return new WaitUntil(() => resultRequest.isDone);

        Debug.Log("Level Deleted");
    }

    public void LaunchEditorMode()
	{
		editor.SetActive(true);
        clearHolder();
        GameObject.FindGameObjectWithTag("PlayMode").SetActive(false);
	}

    public void saveLevelInDatabse()
    {
        Debug.Log("Level Saved");
        UpdateList();

        editor.SetActive(true);
        clearHolder();
        GameObject.FindGameObjectWithTag("PlayMode").SetActive(false);
    }

    private void clearHolder()
    {
        Transform holder = GameObject.FindGameObjectWithTag("Holder").transform;
        foreach (Transform childTransform in holder) Destroy(childTransform.gameObject);
    }
}
