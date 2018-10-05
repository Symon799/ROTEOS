using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class LevelGenerator : MonoBehaviour
{
    public static string levelName;

    public List<GameObject> Objects;
    public Transform parent;

    [Inject]
    private DiContainer _diContainer;

    // Update is called once per frame
    void Start()
    {
        InitializeLevel(ReadLevelJSON());
        SpawnCharacter();
        Debug.Log(_diContainer);
    }

    public void instanciate(Element Element)
    {
        Debug.Log(Element.Rotation);
        _diContainer.InstantiatePrefab(Element.toInstantiate(), Element.Position, Element.Rotation, parent);
    }

    public void instanciate(GameObject GameObject)
    {
        _diContainer.InstantiatePrefab(GameObject, GameObject.transform.position, GameObject.transform.rotation, parent);
    }

    public void SpawnCharacter()
    {
        try
        {
            GameObject start = GameObject.FindGameObjectWithTag("Start");
            GameObject character = GameObject.FindGameObjectWithTag("Player");
            Vector3 position = new Vector3(0, 3, 0) + start.transform.position;
            character.transform.position = position;
        }
        catch
        {
            return;
        }
    }

    public List<Element> ReadLevelJSON()
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
                    Debug.Log(elm.Rotation);
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
            if (parent != null)
            {
                foreach (Transform child in parent)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            Debug.Log("Initialize level...");
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
