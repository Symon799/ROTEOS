using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class CubePlacer : MonoBehaviour
{
    //Public
    public Camera mycam;
    public GameObject[] prefabs;
    public GameObject selectionBloc;
    public int lenght = 40;

    //private
    private Grid grid;
    private GameObject[,,] arr;
    private int[,,] idArr;
    private GameObject currentBloc;
    private GameObject currentSelection;
    public Material transparentMaterial;
    private int currentId = 0;

     //SELECTION ----------------------------------------------------------SELECTION
    
    public GameObject addButton;
    public GameObject removeBtn;
    private bool selectionMode = false;
    private Vector3 pA = Vector3.zero;
    private Vector3 pB = Vector3.zero;
    private GameObject groups;
    private Transform groupSelected;


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
    //------------------------------------------------------------------

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

        //INSTANCE CURRENTSELECTION
        groups = new GameObject();
        groups.name = "Groups";
        //selectionMode = true;
        removeBtn.SetActive(groupSelected);
        addButton.SetActive(false);
    }

    public void setBloc(int id)
    {
        currentId = id;
        Destroy(currentBloc);
    }

    public void removeGroup()
    {
        Destroy(groupSelected.gameObject);
        removeBtn.SetActive(false);

    }
    
    public void switchSelectionMode()
    {
        selectionMode = !selectionMode;
        addButton.SetActive(selectionMode);

        groupSelected = null;
        removeBtn.SetActive(false);
        addButton.SetActive(false);

        if (selectionMode)
            Destroy(currentBloc);
        else
            Destroy(currentSelection);
            

        updateMeshRenderer();
    }

    private void updateMeshRenderer()
    {
        Transform[] groupsChildren = new Transform[groups.transform.childCount];
        for (int i = 0; i < groups.transform.childCount; i++) {
            groupsChildren[i] = groups.transform.GetChild(i);
        }
        foreach (Transform group in groupsChildren)
        {
            Transform[] groupChildren = new Transform[group.transform.childCount];
            for (int i = 0; i < group.transform.childCount; i++)
            {
                if (group.transform.GetChild(i).name == "SelectionCube")
                {
                    group.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = selectionMode;
                }
            }

        }
    }

    public void addGroup()
    {
        GameObject current = Instantiate(currentSelection);
        current.transform.parent = groups.transform;

        Vector3 pa2 = pA;
        Vector3 pb2 = pB;

        int ix = (int)(pa2.x < pb2.x ? pa2.x : pb2.x);
        int iz = (int)(pa2.z < pb2.z ? pa2.z : pb2.z);
        int iy = (int)(pa2.y < pb2.z ? pa2.y : pb2.y);
        
        int fx = (int)(pa2.x >= pb2.x ? pa2.x : pb2.x);
        int fz = (int)(pa2.z >= pb2.z ? pa2.z : pb2.z);
        int fy = (int)(pa2.y >= pb2.z ? pa2.y : pb2.y);

        Debug.Log("x: " + ix + "  y: " + iy + " z: " + iz);
        Debug.Log("fx: " + fx + "  fy: " + fy + " fz: " + fz);
        Debug.Log("ELEMENTS");

        for (int x = ix; x <= fx; x++)
        {
            for (int z = iz; z <= fz; z++)
            {
                for (int y = iy; y <= fy; y++)
                {
                    if (arr[x, y, z])
                        arr[x, y, z].transform.parent = current.transform;
                    Debug.Log("x: " + x + "  y: " + y + " z: " + z);
                }
            }
        }
        Destroy(currentSelection);
        groupSelected = current.transform;
        removeBtn.SetActive(true);
        addButton.SetActive(false);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!selectionMode)
        {
            RaycastHit hitInfo;
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
                rotateBlocY();
            if (Input.GetKeyDown("t"))
                rotateBlocX();
        }
        else
        {
            selection(ray);
        }
        scrollMove();
    }

    private void scrollMove()
    {
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
    }

    private void selection(Ray ray)
    {
        RaycastHit hitInfo;
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hitInfo))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    pA = grid.GetNearestPointOnGrid(hitInfo.point) / 2;
                    //check if it is a group
                    bool selectGroupBool = false;
                    if (groups)
                    {
                        Transform[] groupsChildren = new Transform[groups.transform.childCount];
                        for (int i = 0; i < groups.transform.childCount; i++) {
                            groupsChildren[i] = groups.transform.GetChild(i);
                        }
                        foreach (Transform group in groupsChildren)
                        {
                            Transform[] groupChildren = new Transform[group.transform.childCount];
                            for (int i = 0; i < group.transform.childCount; i++)
                            {
                                if (group.transform.GetChild(i).transform.position == pA * 2)
                                {
                                    selectGroupBool = true;
                                    groupSelected = group;
                                    Destroy(currentSelection);
                                    Debug.Log(group.transform.GetChild(i).name);
                                }
                            }

                        }
                    }

                    if (!selectGroupBool)
                        groupSelected = null;
                    else
                        Destroy(currentSelection);
                    removeBtn.SetActive(groupSelected);

                    if (!groupSelected)
                    {
                        if (!currentSelection)
                            currentSelection = Instantiate(selectionBloc, pA, Quaternion.identity);
                        currentSelection.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                        currentSelection.transform.position = pA * 2;
                    }
                    addButton.SetActive(!groupSelected && currentSelection);
                }
                else if (Input.GetMouseButton(0) && !groupSelected)
                {
                    Vector3 oneVector = new Vector3(1,0,1);
                    pB = grid.GetNearestPointOnGrid(hitInfo.point) / 2;

                    Vector3 pa2 = pA;
                    Vector3 pb2 = pB;

                    pb2.x = pb2.x >= pa2.x ? pb2.x + 1: pb2.x;
                    pb2.z = pb2.z >= pa2.z ? pb2.z + 1: pb2.z;
                    pb2.y = pb2.y >= pa2.y ? pb2.y + 1: pb2.y;

                    pa2.x = pa2.x >= pb2.x ? pa2.x + 1: pa2.x;
                    pa2.z = pa2.z >= pb2.z ? pa2.z + 1: pa2.z;
                    pa2.y = pa2.y >= pb2.y ? pa2.y + 1: pa2.y;

                    Vector3 difference = (pb2 - pa2);
                    difference += new Vector3(difference.x >= 0 ? 0.1f : -0.1f, difference.y >= 0 ? 0.1f : -0.1f, difference.z >= 0 ? 0.1f : -0.1f);
                    currentSelection.transform.localScale = difference;
                    currentSelection.transform.position = new Vector3(pa2.x * 2 + ((pb2.x - pa2.x)) - 1, pa2.y * 2 + ((pb2.y - pa2.y)) - 1, pa2.z * 2 + (pb2.z - pa2.z) - 1);
                }
            }
    }


    private void rotateBlocY()
    {
        currentBloc.transform.Rotate(0, 90, 0, Space.World);
    }

    private void rotateBlocX()
    {
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