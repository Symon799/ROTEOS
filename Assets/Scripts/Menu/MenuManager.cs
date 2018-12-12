using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Inject]
    private IWebRequester _webRequester;
    public GameObject worldButton;
    public GameObject levelButton;

    public GameObject SelectWorld;
    public GameObject SelectLevel;
    public GameObject LevelInfos;
    public GameObject allWorldMenu;
    public GameObject worldMenu;
    public GameObject levelsMenu;
    public TMP_Text levelText;

    public LevelGenerator levelGenerator;
    public Text timeText;
    public Text pointText;

    MetaData levelMetaData;


    // Use this for initialization
    void Start()
    {
    }

    public void StartMenu()
    {
        LoadMetaData();
        LoadLevels();
    }

    public void LoadMetaData()
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Levels/metadata");
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                MetaData loadedData = JsonUtility.FromJson<MetaData>(dataAsJson);

                Debug.Log(dataAsJson);
                Debug.Log(JsonUtility.ToJson(loadedData));
                levelMetaData = loadedData;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
            Debug.Log("Can't find files");
        }
    }

    public void LoadLevels()
    {
        try
        {
            if (levelMetaData == null)
                throw new Exception();
            foreach (Transform child in worldMenu.transform)
                GameObject.Destroy(child.gameObject);
            foreach (var level in levelMetaData.levels)
            {
                GameObject tmp = levelButton;
                var button = tmp.transform.GetComponent<Button>();
                tmp.transform.localScale = new Vector3(2, 2, 2);
                var instantiatedButton = Instantiate(tmp, worldMenu.transform.position, tmp.transform.rotation, worldMenu.transform);
                instantiatedButton.GetComponent<Button>().onClick.AddListener(delegate { SelectLevel.SetActive(false); });
                instantiatedButton.GetComponent<Button>().onClick.AddListener(delegate { LevelInfos.SetActive(true); });
                instantiatedButton.GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(level.name.ToString(), level.id); });
                instantiatedButton.GetComponentInChildren<Text>().text = level.name;
                IconActivator icons = instantiatedButton.GetComponent<IconActivator>();
                if (level.cold)
                    icons.snow = true;
                if (level.rain)
                    icons.rain = true;
                icons.toUpdate();
            }


        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
            Debug.Log("Can't find files");
        }
    }

    private string GetTimeScore(long elapsed_time)
    {
        int min = (int)(elapsed_time / 60);
        int sec = (int)(elapsed_time % 60);
        return (min + "m" + sec + "s");
    }

    public void LoadLevel(string name, long id)
    {
        LevelGenerator.levelName = name;
        LevelGenerator.levelId = id;
        levelText.text = name;
        try
        {
            if (levelMetaData == null)
                throw new Exception();
            JSONScore score = JSONScoreActions.getJSONScore(id);
            if (score != null)
            {
                timeText.text = GetTimeScore(score.seconds);
                pointText.text = score.points.ToString();
            }
            else
            {
                timeText.text = "--m--s";
                pointText.text = "----";
            }

            levelGenerator.InitializeLevelEditor(levelGenerator.ReadLevelJSON());
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
            Debug.Log("Can't generate level");
        }
    }

    public void LaunchLevel()
    {
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync("Level"));
    }
}


