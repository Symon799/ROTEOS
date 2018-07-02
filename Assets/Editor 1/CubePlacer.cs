using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class CubePlacer : MonoBehaviour
{
    public Camera mycam;
    public GameObject[] prefabs;
    public int lenght = 40;


    private Grid grid;
    private GameObject[,,] arr;
    private int[,,] idArr;
    private GameObject currentBloc;
    public Material transparentMaterial;
    private int currentId = 0;


    //JSON ----------------------------------------------------------JSON
    private ElementCollection eltCollection;
    private string levelName = "/Levels/level.json";

    [System.Serializable]
    public class Element
    {
        public long id;
        public Vector3 position;
        public Quaternion rotation;
    }

    [System.Serializable]
    public class ElementCollection
    {
        public List<Element> elements;
    }
    //JSON ------------------------------


    public void setBloc(int id)
    {
        currentId = id;
        Destroy(currentBloc);
    }

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        arr = new GameObject[lenght, lenght, lenght]; 
        idArr = new int[lenght, lenght, lenght];

        for (int x = 0; x < lenght; x++)
            for (int y = 0; y < lenght; y ++)
                for (int z = 0; z < lenght; z++)
                {
                    idArr[x, y, z] = -1;
                    arr[x, y, z] = null;
                }
    }

    private void Update()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        //Scroll position
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 2, Camera.main.transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 2, Camera.main.transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
        }


        //Place cube
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hitInfo))
        {
            if (Input.GetMouseButton(0))
                    PlaceCubeNear(hitInfo.point);

            if (Input.GetMouseButton(2))
                    DeleteCubeNear(hitInfo.point);

            ShowCubeNear(hitInfo.point);
        }
        if (Input.GetKeyDown("r"))
        {         
            rotateBlocY();
        }
        if (Input.GetKeyDown("t"))
        {         
            rotateBlocX();
        }
    }


    private void rotateBlocY()
    {
        Debug.Log("rot");
        currentBloc.transform.Rotate(0, 90, 0, Space.World);
    }

    private void rotateBlocX()
    {
        Debug.Log("rot");
        currentBloc.transform.Rotate(90, 0, 0, Space.World);
    }
    private void PlaceCubeNear(Vector3 clickPoint)
    {
        Vector3 finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        Vector3 gridPosition = finalPosition / 2;

        //A bloc already exist
        if (arr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z])
            return;

        arr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z] = Instantiate(prefabs[currentId], finalPosition, Quaternion.identity);

        //Applying rotation after instantiation
        arr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z].transform.rotation = currentBloc.transform.rotation;
        
        //Add id
        idArr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z] = currentId;

        //Shows grid position
        Debug.Log(finalPosition);
    }

    private void DeleteCubeNear(Vector3 clickPoint)
    {
        Vector3 finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        Vector3 gridPosition = finalPosition / 2;

        GameObject go = arr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z];

        if (go)
        {
            Destroy(go);
            arr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z] = null;
        }
    }

    private void ShowCubeNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        if (!currentBloc)
        {
            currentBloc = Instantiate(prefabs[currentId], finalPosition, Quaternion.identity);
            SetLayerRecursively(currentBloc.transform.gameObject, 2);
        }
        currentBloc.transform.position = finalPosition;
    }


    private void SetLayerRecursively(GameObject obj, int layer) {
        obj.layer = layer;
        obj.GetComponentInChildren<Renderer>().material = transparentMaterial;
 
        foreach (Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void WriteJson()
    {
        eltCollection = new ElementCollection();
        eltCollection.elements = new List<Element>();

        for (int x = 0; x < lenght; x++)
            for (int y = 0; y < lenght; y++)
                for (int z = 0; z < lenght; z++)
                    if (arr[x, y, z])
                        {
                            Element newElt = new Element();
                            newElt.id = idArr[x, y, z];
                            newElt.position = arr[x, y, z].transform.position;
                            newElt.rotation = arr[x, y, z].transform.rotation;
                            eltCollection.elements.Add(newElt);
                        }

        string jsonFile = JsonUtility.ToJson(eltCollection);
        Debug.Log("JSON FILE \n" + jsonFile + "\n");

        //DEBUG ARR
        for (int x = 0; x < lenght; x++)
            for (int y = 0; y < lenght; y++)
                for (int z = 0; z < lenght; z++)
                    if (arr[x, y, z])
                        Debug.Log("X = " + x + " Y = " + y + " Z = " + z);
        //DEBUG ARR

        File.WriteAllText(Application.dataPath + levelName, jsonFile);
    }

    public void LoadJson()
    {
        string content = File.ReadAllText(Application.dataPath + levelName);
        eltCollection = JsonUtility.FromJson<ElementCollection>(content);

        for (int x = 0; x < lenght; x++)
            for (int y = 0; y < lenght; y++)
                for (int z = 0; z < lenght; z++)
                    if (arr[x, y, z])
                    {
                        Destroy(arr[x, y, z]);
                        arr[x, y, z] = null;
                    }

        foreach (Element elt in eltCollection.elements)
        {
            int id = (int)elt.id;
            arr[(int)elt.position.x / 2, (int)elt.position.y / 2, (int)elt.position.z / 2] = Instantiate(prefabs[id], elt.position, elt.rotation);
            idArr[(int)elt.position.x / 2, (int)elt.position.y / 2, (int)elt.position.z / 2] = id;
        }
    }
}