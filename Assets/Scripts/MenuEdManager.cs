using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
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
    private Level levelToSave = null;

    // Save Tab
    public InputField nameField;
    public InputField descriptionField;
	
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
        UpdateList();
    }

    //working
	public IEnumerator getLevels(long idUser)
    {
        clearList();

        Debug.Log("Getting levels for user " + idUser + "...");
        string sailsUrl = "https://secure-sands-20186.herokuapp.com/getalllevelbyiduserwithjson";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters.Add("iduser", idUser.ToString());


        UnityWebRequest resultRequest =_webRequester.Get(sailsUrl, parameters);

        yield return new WaitUntil(() => resultRequest.isDone);
        Debug.Log(resultRequest.responseCode + " Result : " + resultRequest.downloadHandler.text);

        levelsObj = JsonUtility.FromJson<Levels>("{\"levels\":" + resultRequest.downloadHandler.text + "}");

        foreach(Level lvl in levelsObj.levels)
        {
            GameObject button = Instantiate(levelButtonGroup, grid);
            button.GetComponent<ButtonUpdater>().setValues(lvl, menuEditor, editor);
        }
    }

    public void UpdateList()
    {
        StartCoroutine(getLevels(AccountManager.idCurrentUser));
    }

    private void clearList()
    {
        foreach (Transform child in grid.transform)
            GameObject.Destroy(child.gameObject);
    }

    //working
    public IEnumerator addLevel(string name, string description, string weather, int maxScore, string json)
    {
        clearList();
        Debug.Log("Adding level to user " + AccountManager.idCurrentUser + "...");
        string sailsUrl = "https://immense-lake-57494.herokuapp.com/levels";

        AddLevel body = new AddLevel();
        body.name = name;
        body.json = json;
        body.createdAt = null;
        body.updatedAt = null;
        body.description = description;
        body.weather_savior = weather;
        body.maxScore = maxScore;
        body.idUser = AccountManager.idCurrentUser;

        string bodyJson = JsonUtility.ToJson(body);

        yield return StartCoroutine(_webRequester.PostComplete(sailsUrl, bodyJson));
        Debug.Log("Level Added");
        UpdateList();
    }
    public IEnumerator deleteLevel(long idLevel)
    {
        clearList();
        Debug.Log("Deleting level " + idLevel + "...");
        string sailsUrl = "https://immense-lake-57494.herokuapp.com/levels";


        UnityWebRequest resultRequest = _webRequester.Delete(sailsUrl, idLevel);

        yield return new WaitUntil(() => resultRequest.isDone);

        Debug.Log("Level Deleted");
    }

    public IEnumerator deleteLevelButton(long idLevel, GameObject popUp)
    {
        Debug.Log("Deleting level " + idLevel + "...");
        string sailsUrl = "https://immense-lake-57494.herokuapp.com/levels";


        UnityWebRequest resultRequest = _webRequester.Delete(sailsUrl, idLevel);

        yield return new WaitUntil(() => resultRequest.isDone);
        Destroy(popUp);
        Debug.Log("Level Deleted");
        UpdateList();
    }

    public void LaunchEditorMode()
	{
		editor.SetActive(true);
        clearHolder();
        GameObject.FindGameObjectWithTag("PlayMode").SetActive(false);
	}

    public void launchPopUpSave()
    {
        if (levelToSave != null)
        {
            nameField.text = levelToSave.namelevel;
            descriptionField.text = levelToSave.descriptionlevel;
        }
    }

    public void saveLevelInDatabse()
    {
        string filePath = Path.Combine(Application.dataPath, "Levels/level.json");
        if (!File.Exists(filePath))
            return;

        string levelJsonStr = File.ReadAllText(filePath);
        if (levelToSave == null)
            levelToSave = new Level();
        else
            StartCoroutine(deleteLevel(levelToSave.idlevel));

        StartCoroutine(addLevel(nameField.text, descriptionField.text, "Clear", levelToSave.maxscorelevel, levelJsonStr));

        clearHolder();
        GameObject.FindGameObjectWithTag("PlayMode").SetActive(false);
        menuEditor.SetActive(true);
    }

    private void clearHolder()
    {
        GameObject.FindGameObjectWithTag("Managers").GetComponentInChildren<LevelManager>().resetLevel();
        Transform holder = GameObject.FindGameObjectWithTag("Holder").transform;
        foreach (Transform childTransform in holder) Destroy(childTransform.gameObject);
        levelToSave = null;
        nameField.text = "";
        descriptionField.text = "";
    }

    public void launchNewLevel()
    {
		menuEditor.SetActive(false);
		editor.SetActive(true);
		editor.GetComponentInChildren<CubePlacer>().currentLevel = null;
		editor.GetComponentInChildren<CubePlacer>().LoadJson();
    }

    public void setLevelToSave(Level save)
    {
        levelToSave = save;
    }
}
