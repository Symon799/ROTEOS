using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class LevelGenerator : MonoBehaviour
{

    public static string levelName
    {
        get
        {
            return levelName;
        }
        set
        {
            levelName = value;
        }
    }

    public List<GameObject> Objects;

    [Inject]
    private DiContainer _diContainer;

    // Update is called once per frame
    void Start()
    {
		InitializeLevel(ReadLevelJSON("level"));
    }

    public void instanciate(Element Element)
    {

        _diContainer.InstantiatePrefab(Element.toInstantiate(), Element.Position, Element.Rotation, null);
    }

    public void instanciate(GameObject GameObject)
    {
        _diContainer.InstantiatePrefab(GameObject, GameObject.transform.position, GameObject.transform.rotation, null);
    }

    public void SpawnCharacter()
    {

    }

    public List<Element> ReadLevelJSON(string levelName)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Levels/" + levelName);
			Debug.Log(filePath);
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                JsonLevel loadedData = JsonUtility.FromJson<JsonLevel>(dataAsJson);

				Debug.Log(dataAsJson);
                List<Element> Elements = new List<Element>();
				Debug.Log(JsonUtility.ToJson(loadedData));
                foreach (var element in loadedData.elements)
                {
                    Element elm = new Element(idToGameObject(element.id));
                    elm.Position = new Vector3(element.position.x, element.position.y, element.position.z);
                    elm.Rotation = new Quaternion(element.rotation.x, element.rotation.y, element.rotation.z, element.rotation.w);
                    Elements.Add(elm);
                }
                return Elements;
            }
			else
			{
				Debug.Log("Can't find file !");
			}
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
            Debug.Log("Can't find designed level");
            return null;
        }


        return null;
    }

    public bool InitializeLevel(List<Element> elements)
    {
        try
        {
            foreach (var elm in elements)
            {
                instanciate(elm);
            }
			return true;
        }
		catch
		{
			return false;
		}
    }

    public GameObject idToGameObject(int id)
    {
        return Objects[id];
    }
}
