using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class CubePlacer : MonoBehaviour
{
    //PUBLIC
    public GameObject[] prefabs;
    public GameObject selectionBloc;
    public int lenght = 40;

    //PRIVATE
    private Grid grid;
    private GameObject[,,] arr;
    private int[,,] idArr;
    private GameObject currentBloc;
    private GameObject currentSelection;
    public Material transparentMaterial;
    private int currentId = 0;

     //SELECTION ----------------------------------------------------------SELECTION
    
    public GameObject addButton;
    public GameObject popUpGroup;
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
        popUpGroup.SetActive(groupSelected);
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
        popUpGroup.SetActive(false);

    }
    
    public void switchSelectionMode()
    {
        selectionMode = !selectionMode;
        addButton.SetActive(selectionMode);

        groupSelected = null;
        popUpGroup.SetActive(false);
        addButton.SetActive(false);

        if (selectionMode)
            Destroy(currentBloc);
        else
            Destroy(currentSelection);
            
        updateMeshRenderer();
    }

    // Enable or not blue selection viusal
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

        for (int x = ix; x <= fx; x++)
            for (int z = iz; z <= fz; z++)
                for (int y = iy; y <= fy; y++)
                    if (arr[x, y, z])
                        arr[x, y, z].transform.parent = current.transform;
        
        Destroy(currentSelection);
        groupSelected = current.transform;
        popUpGroup.SetActive(true);
        addButton.SetActive(false);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!selectionMode)
            CubePlace(ray);
        else
            selection(ray);
        scrollMove();
    }

    private void CubePlace(Ray ray)
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
                    getSelectedGroup();
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

                    Vector3 pa2 = pA, pb2 = pB;
                    getSelectionPointVisual(ref pa2, ref pb2);
                    
                    Vector3 diff = (pb2 - pa2);
                    diff += new Vector3(diff.x >= 0 ? 0.01f : -0.01f, diff.y >= 0 ? 0.01f : -0.01f, diff.z >= 0 ? 0.01f : -0.01f);
                    currentSelection.transform.localScale = diff;
                    currentSelection.transform.position = new Vector3(pa2.x * 2 + ((pb2.x - pa2.x)) - 1, pa2.y * 2 + ((pb2.y - pa2.y)) - 1, pa2.z * 2 + (pb2.z - pa2.z) - 1);
                }
            }
    }

    private void getSelectedGroup()
    {
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
                    }
                }
            }
        }

        if (!selectGroupBool)
            groupSelected = null;
        else
            Destroy(currentSelection);

        popUpGroup.SetActive(groupSelected);
    }

    //Get selection points position to display correctly
    private void getSelectionPointVisual(ref Vector3 pa2, ref Vector3 pb2)
    {
        pb2.x = pb2.x >= pa2.x ? pb2.x + 1: pb2.x;
        pb2.z = pb2.z >= pa2.z ? pb2.z + 1: pb2.z;
        pb2.y = pb2.y >= pa2.y ? pb2.y + 1: pb2.y;

        pa2.x = pa2.x >= pb2.x ? pa2.x + 1: pa2.x;
        pa2.z = pa2.z >= pb2.z ? pa2.z + 1: pa2.z;
        pa2.y = pa2.y >= pb2.y ? pa2.y + 1: pa2.y;
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
            SetTransparentRecursively(currentBloc.transform.gameObject, 2);
        }
        currentBloc.transform.position = finalPosition;
    }


    private void SetTransparentRecursively(GameObject obj, int layer) {
        obj.layer = layer;
        obj.GetComponentInChildren<Renderer>().material = transparentMaterial;
 
        foreach (Transform child in obj.transform) {
            SetTransparentRecursively(child.gameObject, layer);
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