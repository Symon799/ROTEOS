using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject worldButton;
    public GameObject levelButton;

    public GameObject SelectWorld;
    public GameObject SelectLevel;
    public GameObject LevelInfos;
    public GameObject allWorldMenu;
    public GameObject worldMenu;
    public GameObject levelsMenu;

    public LevelGenerator levelGenerator;

    MetaData levelMetaData;

    // Use this for initialization
    void Start()
    {
        LoadMetaData();
        LoadAllWorlds();
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

    public void LoadAllWorlds()
    {
        try
        {
            if (levelMetaData == null)
                throw new Exception();
            foreach (var world in levelMetaData.worlds)
            {
                GameObject tmp = worldButton;
                var button = tmp.transform.GetComponent<Button>();
                tmp.transform.localScale = new Vector3(2, 2, 2);
                var instantiatedButton = Instantiate(tmp, allWorldMenu.transform.position, tmp.transform.rotation, allWorldMenu.transform);
                instantiatedButton.GetComponent<Button>().onClick.AddListener(() => SelectWorld.SetActive(false));
                instantiatedButton.GetComponent<Button>().onClick.AddListener(() => SelectLevel.SetActive(true));
                instantiatedButton.GetComponent<Button>().onClick.AddListener(() => LoadWorldLevels(world.id));
                instantiatedButton.GetComponentInChildren<Text>().text = world.name;
            }

        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
            Debug.Log("Can't find files");
        }
    }

    public void LoadWorldLevels(int id)
    {
        try
        {
            if (levelMetaData == null)
                throw new Exception();
            world wrld = levelMetaData.worlds.Find(w => w.id == id);
            foreach (Transform child in worldMenu.transform)
                GameObject.Destroy(child.gameObject);
            foreach (var level in wrld.levels)
            {
                GameObject tmp = levelButton;
                var button = tmp.transform.GetComponent<Button>();
                tmp.transform.localScale = new Vector3(2, 2, 2);
                var instantiatedButton = Instantiate(tmp, worldMenu.transform.position, tmp.transform.rotation, worldMenu.transform);
                instantiatedButton.GetComponent<Button>().onClick.AddListener(delegate { SelectLevel.SetActive(false); });
                instantiatedButton.GetComponent<Button>().onClick.AddListener(delegate { LevelInfos.SetActive(true); });
                instantiatedButton.GetComponent<Button>().onClick.AddListener(delegate { LoadLevel(level.id); });
                IconActivator icons = instantiatedButton.GetComponent<IconActivator>();
                if (level.cold)
                    icons.snow = true;
                if (level.rain)
                    icons.rain = true;
            }

        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
            Debug.Log("Can't find files");
        }
    }

    public void LoadLevel(string id)
    {
        LevelGenerator.levelName = id;
        try
        {
            if (levelMetaData == null)
                throw new Exception();
            levelGenerator.InitializeLevel(levelGenerator.ReadLevelJSON());
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
            Debug.Log("Can't generate level");
        }
    }

    public void LaunchLevel()
    {
        SceneManager.LoadScene("Level");
    }
}


