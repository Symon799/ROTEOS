using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class CubePlacer : MonoBehaviour
{
    public GameObject bloc;
    public Camera mycam;

    public int lenght = 40;

    private Grid grid;
    private GameObject[,,] arr;
    private GameObject currentBloc;


    //JSON
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

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        var finalPosition = grid.GetNearestPointOnGrid(Vector3.zero);
        currentBloc = Instantiate(bloc, finalPosition, Quaternion.identity);

        arr = new GameObject[lenght, lenght, lenght];

        for (int x = 0; x < lenght; x++)
            for (int y = 0; y < lenght; y ++)
                for (int z = 0; z < lenght; z++)
                arr[x, y, z] = null;

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
        if (Input.GetMouseButtonDown(0))
            if (Physics.Raycast(ray, out hitInfo))
                PlaceCubeNear(hitInfo.point);

        if (Input.GetMouseButtonDown(2))
            if (Physics.Raycast(ray, out hitInfo))
                DeleteCubeNear(hitInfo.point);


        //Show cube
        if (Physics.Raycast(ray, out hitInfo))
            ShowCubeNear(hitInfo.point);

        //WriteJson
        if (Input.GetKeyDown(KeyCode.Escape))
            WriteJson();
    }


    private void PlaceCubeNear(Vector3 clickPoint)
    {
        Vector3 finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        Vector3 gridPosition = finalPosition / 2;

        //A bloc already exist
        if (arr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z])
            return;

        arr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z] = Instantiate(bloc, finalPosition, Quaternion.identity);

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
        currentBloc.transform.position = finalPosition;
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
                            newElt.id = 1; //Id of the prefab
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
            arr[(int)elt.position.x / 2, (int)elt.position.y / 2, (int)elt.position.z / 2] = Instantiate(bloc, elt.position, elt.rotation);
        }
    }
}