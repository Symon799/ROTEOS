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
    public Transform target;


    public void InitializeGame()
    {
        InitializeLevel(ReadLevelJSON());
        SpawnCharacter();
    }

    public GameObject instanciate(Element Element)
    {
        return _diContainer.InstantiatePrefab(Element.toInstantiate(), Element.Position, Element.Rotation, parent);
    }

    public GameObject instanciate(GameObject GameObject)
    {
        return _diContainer.InstantiatePrefab(GameObject, GameObject.transform.localPosition, GameObject.transform.rotation, parent);
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

    public LevelToLoad ReadLevelJSON()
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
                List<Group> Groups = new List<Group>();
                List<Interactable> Interactables = new List<Interactable>();
                //Debug.Log(JsonUtility.ToJson(loadedData));
                foreach (var element in loadedData.elements)
                {
                    Element elm = new Element(idToGameObject(element.id));
                    elm.Position = new Vector3(element.position.x - 20.5f, element.position.y, element.position.z - 20.5f);
                    elm.Rotation = new Quaternion(element.rotation.x, element.rotation.y, element.rotation.z, element.rotation.w);
                    //Debug.Log(elm.Rotation);
                    Elements.Add(elm);
                }

                foreach (var group in loadedData.groups)
                {
                    Group grp = new Group();
                    grp.component = new Component();
                    grp.component.channel = group.component.channel;
                    grp.component.id = group.component.id;
                    grp.component.position = new Vector3(group.component.position.x - 20.5f, group.component.position.y, group.component.position.z - 20.5f);
                    grp.component.speed = group.component.speed;
                    grp.pA = new Vector3((group.pA.x * 2) - 20.5f, group.pA.y * 2, (group.pA.z * 2) - 20.5f);
                    grp.pB = new Vector3((group.pB.x * 2) - 20.5f, group.pB.y * 2, (group.pB.z * 2) - 20.5f);
                    //Debug.Log(elm.Rotation);
                    Groups.Add(grp);
                }

                foreach (var interactable in loadedData.interactables)
                {
                    Interactable inte = new Interactable();
                    inte.position = new Vector3(interactable.pos.x - 20.5f, interactable.pos.y, interactable.pos.z - 20.5f);
                    inte.channel = interactable.channel;
                    //Debug.Log(elm.Rotation);
                    Interactables.Add(inte);
                }
                LevelToLoad lvl = new LevelToLoad();
                lvl.groups = Groups;
                lvl.elements = Elements;
                lvl.interactables = Interactables;
                return lvl;
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

    public bool InitializeLevel(LevelToLoad lvl)
    {
        try
        {
            if (parent != null)
                foreach (Transform child in parent)
                    GameObject.Destroy(child.gameObject);

            Debug.Log("Initialize level...");
            float highestY = 0;
            List<GameObject> instanciated = new List<GameObject>();
            foreach (var elm in lvl.elements)
            {
                if (elm.Position.y > highestY)
                    highestY = elm.Position.y;
                instanciated.Add(instanciate(elm));
            }
            foreach (var grp in lvl.groups)
            {
                Debug.Log(JsonUtility.ToJson(grp));
                createGroup(grp, instanciated);
            }
            foreach (var inte in lvl.interactables)
            {
                Debug.Log(JsonUtility.ToJson(inte));
                applyChannel(inte, instanciated);
            }
            if (lightManager != null)
            {
                lightManager.SetDistance(highestY);
                lightManager.setPlanetary();
            }
            if (target != null)
            {
                parent.SetParent(target);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }

    }

    public bool InitializeLevelEditor(LevelToLoad lvl)
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
            foreach (var elm in lvl.elements)
            {
                Debug.Log(elm.toInstantiate().transform.localScale);
                var obj = elm.toInstantiate();
                obj.transform.localScale = new Vector3(1, 1, 1);
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

    public bool isInBounds(Vector3 firstPoint, Vector3 secondPoint, Vector3 position)
    {
        Debug.Log("Position : " + position + " | pA : " + firstPoint + " | pB : " + secondPoint);
        return (position.x <= Math.Max(firstPoint.x, secondPoint.x) && position.x >= Math.Min(firstPoint.x, secondPoint.x))
            && (position.y <= Math.Max(firstPoint.y, secondPoint.y) && position.y >= Math.Min(firstPoint.y, secondPoint.y))
            && (position.z <= Math.Max(firstPoint.z, secondPoint.z) && position.z >= Math.Min(firstPoint.z, secondPoint.z));
    }

    public Vector3 findCenter(Transform parent)
    {
        Vector3 result = new Vector3(0, 0, 0);
        for (int i = 0; i < parent.childCount; i++)
        {
            result += parent.GetChild(i).position;
        }
        if (parent.childCount > 0)
            result = result / (parent.childCount);
        return result;
    }

    public Vector3 findCenter(List<GameObject> childs)
    {
        Vector3 result = new Vector3(0, 0, 0);
        foreach (var child in childs)
        {
            Debug.Log("Position : " + child.transform.position);
            result += child.transform.position;
            Debug.Log("Result : " + result);
        }
        if (childs.Count > 0)
            result = result / (childs.Count);
        Debug.Log("Final result : " + result);
        return result;
    }

    public void createGroup(Group group, List<GameObject> objects)
    {
        GameObject result = new GameObject("Group");
        List<GameObject> included = new List<GameObject>();
        foreach (var tmp in objects)
        {
            if (isInBounds(group.pA, group.pB, tmp.transform.position))
            {
                included.Add(tmp);
            }
        }
        result.transform.position = findCenter(included);
        switch (group.component.id)
        {
            case 1:
                Move move = result.AddComponent(typeof(Move)) as Move;
                move.channel = (Channel)group.component.channel;
                move.Speed = group.component.speed;
                move.MoveToPosition = group.component.position;
                break;
            case 2: 
                Rotate rotate = result.AddComponent(typeof(Rotate)) as Rotate;
                rotate.channel = (Channel)group.component.channel;
                rotate.Speed = group.component.speed;
                rotate.RotateTo = group.component.position;
                break;
            default:
                break;
        }
        GameObject instanciated = this.instanciate(result);
        foreach (var child in included)
        {
            child.transform.parent = instanciated.transform;
        }
    }

    public void applyChannel(Interactable interactable, List<GameObject> objects)
    {
        foreach (var obj in objects)
        {
            if (obj.transform.position == interactable.position)
            {
                obj.GetComponentInChildren<EventClient>().channel = (Channel)interactable.channel;
                return;
            }
        }
    }

}

public class Group
{
    public Vector3 pA;

    public Vector3 pB;

    public Component component;
}

public class Component
{
    public int id;
    public Vector3 position;
    public float speed;
    public int channel;
}

public class Interactable 
{
    public Vector3 position;
    public int channel;
}

public class LevelToLoad
{
    public List<Element> elements;
    public List<Group> groups;
    public List<Interactable> interactables;
}