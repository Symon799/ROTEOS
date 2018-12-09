using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class LevelGenerator : MonoBehaviour
{
    public static string levelName;
    public static long levelId;

    public List<GameObject> Objects;
    public Transform parent;
    public LightManager lightManager;

    public GameObject playerPrefab;

    [Inject]
    private DiContainer _diContainer;


    public void InitializeGame()
    {
        InitializeLevel(ReadLevelJSON());
        SpawnCharacter();
    }

    public void instanciate(Element Element)
    {
        _diContainer.InstantiatePrefab(Element.toInstantiate(), Element.Position, Element.Rotation, parent);
    }

    public void instanciate(GameObject GameObject)
    {
        _diContainer.InstantiatePrefab(GameObject, GameObject.transform.localPosition, GameObject.transform.rotation, parent);
    }

    public void SpawnCharacter()
    {
        try
        {
            Debug.Log("Create character");
            GameObject start = GameObject.FindGameObjectWithTag("Start");
            GameObject character = GameObject.Instantiate(playerPrefab);
            Vector3 position = new Vector3(0, 3, 0) + start.transform.position;
            character.transform.position = position;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return;
        }
    }

    public List<Element> ReadLevelJSON()
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Levels/" + levelName);
            //string filePath = Path.Combine(Application.dataPath, "Levels/level.json");
            Debug.Log(filePath);
            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                JsonLevel loadedData = JsonUtility.FromJson<JsonLevel>(dataAsJson);

                //Debug.Log(dataAsJson);
                List<Element> Elements = new List<Element>();
                //Debug.Log(JsonUtility.ToJson(loadedData));
                foreach (var element in loadedData.elements)
                {
                    Element elm = new Element(idToGameObject(element.id));
                    elm.Position = new Vector3(element.position.x - 20.5f, element.position.y, element.position.z - 20.5f);
                    elm.Rotation = new Quaternion(element.rotation.x, element.rotation.y, element.rotation.z, element.rotation.w);
                    //Debug.Log(elm.Rotation);
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
                foreach (Transform child in parent)
                    GameObject.Destroy(child.gameObject);

            Debug.Log("Initialize level...");
            float highestY = 0;
            foreach (var elm in elements)
            {
                if (elm.Position.y > highestY)
                    highestY = elm.Position.y;
                instanciate(elm);
            }
            if (lightManager != null)
            {
                lightManager.SetDistance(highestY);
                lightManager.setPlanetary();
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }
        
    }

    public bool InitializeLevelEditor(List<Element> elements)
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
               Debug.Log(elm.toInstantiate().transform.localScale);
               var obj = elm.toInstantiate();
               obj.transform.localScale = new Vector3(1, 1 ,1);
                var tmp = GameObject.Instantiate(obj);
                tmp.transform.SetParent(parent, false);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }
    }

    public GameObject idToGameObject(int id)
    {
        return Objects[id];
    }

}
