using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;


public class CubePlacer : MonoBehaviour
{
    //PUBLIC
    public GameObject[] prefabs;
    public GameObject selectionBloc;
    public int lenght = 40;

    public Level currentLevel;
    public GameObject menuEditor;
	public GameObject editor;



    //PRIVATE
    private Grid grid;
    private GameObject[,,] arr;
    private int[,,] idArr;
    private GameObject currentBloc;
    private GameObject currentSelection;
    public Material transparentMaterial;
    private int currentId = 0;

     //PLLLAYMODE ---------------------------------------------------------- PLAYMODE

    public GameObject playMode;

     //SELECTION ----------------------------------------------------------SELECTION
    
    public GameObject addButton;
    public GameObject popUpGroup;
    private Dropdown dropScript, dropChannel, dropButtonChannel;
    private InputField fieldSpeed, fieldX, fieldY, fieldZ;
    private bool selectionMode = false;
    private Vector3 pA = Vector3.zero, pB = Vector3.zero;
    private GameObject levelObj;
    private GameObject groupsObj;
    private Transform groupSelected;
    private Transform ScriptDetails;
    private GameObject interactableGameObject;

    public class Group
    {
        public GameObject gameObject;
        public int id;
        public Vector3 pos;
        public float speed;
        public int channel;
        public Vector3 pA, pB;
    }

    public class Interactable
    {
        public GameObject gameObject;
        public int channel;
    }

    private List<Group> groups;
    private List<Interactable> interactables;

    //JSON ----------------------------------------------------------JSON
    private ElementCollection eltCollection;
    private string levelName = "/Levels/level.json";

    
    //------------------------------------------------------------------

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        arr = new GameObject[lenght, lenght, lenght]; 
        idArr = new int[lenght, lenght, lenght];
        levelObj = new GameObject();
        groupsObj = new GameObject();
        groupsObj.transform.parent = levelObj.transform;
        levelObj.transform.parent = GameObject.Find("Editor").transform;

        groups = new List<Group>();
        interactables = new List<Interactable>();
        levelObj.name = "levelObj";
        groupsObj.name = "Groups";

        for (int x = 0; x < lenght; x++)
            for (int y = 0; y < lenght; y ++)
                for (int z = 0; z < lenght; z++)
                {
                    idArr[x, y, z] = -1;
                    arr[x, y, z] = null;
                }

        initUi();
    }

    private void initUi()
    {
        Dropdown[] dList = popUpGroup.GetComponentsInChildren<Dropdown>();
        foreach (Dropdown d in dList)
        {
            if (d.gameObject.name == "ScriptSelect")
                dropScript = d;
            else if (d.gameObject.name == "Channel")
                dropChannel = d;
        }
        dropButtonChannel = popUpGroup.transform.parent.Find("smallPopUpInteract").GetComponentInChildren<Dropdown>();
        InputField[] fieldList = popUpGroup.GetComponentsInChildren<InputField>();
        foreach (InputField i in fieldList)
        {
            if (i.gameObject.name == "Speed")
                fieldSpeed = i;
            else if (i.gameObject.name == "X")
                fieldX = i;
            else if (i.gameObject.name == "Y")
                fieldY = i;
            else if (i.gameObject.name == "Z")
                fieldZ = i;
        }

        popUpGroup.SetActive(groupSelected);
        addButton.SetActive(false);
        dropButtonChannel.gameObject.transform.parent.gameObject.SetActive(false);
        ScriptDetails = popUpGroup.transform.Find("ScriptDetails");
        updateOutline();
    }

    public void setBloc(int id)
    {
        if (selectionMode)
            switchSelectionMode();
        currentId = id;
        Destroy(currentBloc);
    }

    public void DeleteGroup()
    {
        removeFromGroup(groupSelected.gameObject);

        //Clean interactabes
        foreach (Interactable i in interactables.ToList())
            if (!i.gameObject)
                interactables.Remove(i);

        popUpGroup.SetActive(false);
        updateOutline();
    }

    private Group getComponentInGroups(GameObject go)
    {
        foreach (Group ge in groups)
            if (go.transform.position == ge.gameObject.transform.position)
                return ge;
        return null;
    }

    private void setComponentInGroups(Transform go, int id, Vector3 pos, float speed, int channel)
    {
        foreach (Group ge in groups)
        {
            if (go.position == ge.gameObject.transform.position)
            {
                ge.id = id;
                ge.pos = pos;
                ge.speed = speed;
                ge.channel = channel;
                break;
            }
        }
    }

    public void setButtonChannel()
    {
        foreach (Interactable i in interactables)
        {
            if (interactableGameObject.transform.position == i.gameObject.transform.position)
            {
                i.gameObject = interactableGameObject;
                i.channel = dropButtonChannel.value;
            }
        }
    }

    public void SetComponent()
    {
        setComponentInGroups(groupSelected,
                             dropScript.value,
                             new Vector3(float.Parse(fieldX.text), float.Parse(fieldY.text), float.Parse(fieldZ.text)),
                             float.Parse(fieldSpeed.text), dropChannel.value);

        //Will be useful later
        /*
        if (dropdown.value == 0)
        {
            var components = groupSelected.GetComponents(typeof(Move)).Concat(groupSelected.GetComponents(typeof(Rotate)));
            foreach (Component c in components)
                Destroy(c);
        }
        else if (dropdown.value == 1)
        {
            var components = groupSelected.GetComponents(typeof(Rotate));
            foreach (Component c in components)
                Destroy(c);

            groupSelected.gameObject.AddComponent(typeof(Move));

        }
        else if (dropdown.value == 2)
        {
            var components = groupSelected.GetComponents(typeof(Move));
            foreach (Component c in components)
                Destroy(c);

            groupSelected.gameObject.AddComponent(typeof(Rotate));
        }
        */
    }

    //Remove gameObject from groups and Detroy it
    private void removeFromGroup(GameObject go)
    {
        foreach (Group ge in groups.ToList())
        {
            if (go.transform.position == ge.gameObject.transform.position)
            {
                groups.Remove(ge);
                Destroy(go);
            }
        }
    }

    public void DissolveGroup()
    {
        Destroy(groupSelected.transform.Find("SelectionCube").gameObject);

        Transform[] groupChildren = new Transform[groupSelected.transform.childCount];
        for (int i = 0; i < groupSelected.transform.childCount; i++)
            groupChildren[i] = groupSelected.transform.GetChild(i);

        foreach (Transform t in groupChildren)
            t.parent = t.parent.parent.parent;

        removeFromGroup(groupSelected.gameObject);
        popUpGroup.SetActive(false);
    }
    
    public void switchSelectionMode()
    {
        selectionMode = !selectionMode;
        addButton.SetActive(selectionMode);

        groupSelected = null;
        popUpGroup.SetActive(false);
        addButton.SetActive(false);
        updateOutline();

        if (selectionMode)
            Destroy(currentBloc);
        else
            Destroy(currentSelection);
            
        updateMeshRenderer();
    }

    // Enable or not blue selection viusal
    private void updateMeshRenderer()
    {
        Transform[] groupsChildren = new Transform[groupsObj.transform.childCount];
        for (int i = 0; i < groupsObj.transform.childCount; i++) {
            groupsChildren[i] = groupsObj.transform.GetChild(i);
        }
        foreach (Transform group in groupsChildren)
        {
            Transform[] groupChildren = new Transform[group.transform.childCount];
            for (int i = 0; i < group.transform.childCount; i++)
            {
                if (group.transform.GetChild(i).name == "SelectionCube")
                {
                    group.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = selectionMode;
                    group.transform.GetChild(i).GetComponent<BoxCollider>().enabled = selectionMode;
                }
            }
        }
    }

    private void updateOutline()
    {
        Transform[] groupsChildren = new Transform[groupsObj.transform.childCount];
        for (int i = 0; i < groupsObj.transform.childCount; i++) {
            groupsChildren[i] = groupsObj.transform.GetChild(i);
        }
        foreach (Transform group in groupsChildren)
        {
            Transform[] groupChildren = new Transform[group.transform.childCount];
            for (int i = 0; i < group.transform.childCount; i++)
            {
                if (group.transform.GetChild(i).name == "SelectionCube" && group.transform.GetChild(i).GetComponent<cakeslice.Outline>())
                {
                    group.transform.GetChild(i).GetComponent<cakeslice.Outline>().enabled = false;
                }
            }
        }
        if (popUpGroup.activeSelf)
        {
            setPopUpValues();
            groupSelected.transform.Find("SelectionCube").gameObject.GetComponent<cakeslice.Outline>().enabled = true;
        }
    }

    public void addGroup()
    {
        GameObject current = Instantiate(currentSelection);
        current.transform.parent = groupsObj.transform;

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

        groupSelected = current.transform;
        GameObject selectionCube = groupSelected.transform.Find("SelectionCube").gameObject;

        selectionCube.AddComponent(typeof(cakeslice.Outline)).GetComponent<cakeslice.Outline>().color = 3;
        selectionCube.AddComponent(typeof(BoxCollider));
        Destroy(currentSelection);
        addGroupElement(current);
        setPopUpValues();
        popUpGroup.SetActive(true);
        addButton.SetActive(false);
        updateOutline();
    }

    public void updateGroupComposition()
    {
        foreach (Group gr in groups)
        {
            Vector3 pa2 = gr.pA;
            Vector3 pb2 = gr.pB;

            int ix = (int)(pa2.x < pb2.x ? pa2.x : pb2.x);
            int iz = (int)(pa2.z < pb2.z ? pa2.z : pb2.z);
            int iy = (int)(pa2.y < pb2.z ? pa2.y : pb2.y);
            
            int fx = (int)(pa2.x >= pb2.x ? pa2.x : pb2.x);
            int fz = (int)(pa2.z >= pb2.z ? pa2.z : pb2.z);
            int fy = (int)(pa2.y >= pb2.z ? pa2.y : pb2.y);

            for (int x = ix; x <= fx; x++)
                for (int z = iz; z <= fz; z++)
                    for (int y = iy; y <= fy; y++)
                    {
                        if (arr[x, y, z])
                            arr[x, y, z].transform.parent = gr.gameObject.transform;
                    }
        }
    }

    private void addGroupElement(GameObject go)
    {
        Group ge = new Group();
        ge.gameObject = go;
        ge.id = 0;
        ge.pos = Vector3.zero;
        ge.speed = 1;
        ge.pA = pA;
        ge.pB = pB;
        groups.Add(ge);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!selectionMode)
            CubePlace(ray);
        else
            selection(ray);
        scrollMove();
        
        // Show component details only if not "No Script"
        ScriptDetails.gameObject.SetActive(dropScript.value != 0);
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
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && transform.position.y < lenght)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 2, Camera.main.transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && transform.position.y > 1) // semi-bloc
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 2, Camera.main.transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
        }
    }

    private bool valueInRange(float value, float lim1, float lim2)
    {
        float min = lim1 < lim2 ? lim1 : lim2;
        float max = lim1 > lim2 ? lim1 : lim2;
        return (value >= min) && (value <= max);
    }

    private bool isOverlapping()
    {
        //HERE
        foreach (Group gr in groups)
        {
            bool xOverlap = valueInRange(pA.x, gr.pA.x, gr.pB.x) || valueInRange(gr.pA.x, pA.x, pB.x)
                            || valueInRange(pB.x, gr.pA.x, gr.pB.x) || valueInRange(gr.pB.x, pA.x, pB.x);

            bool yOverlap = valueInRange(pA.y, gr.pA.y, gr.pB.y) || valueInRange(gr.pA.y, pA.y, pB.y)
                            || valueInRange(pB.y, gr.pA.y, gr.pB.y) || valueInRange(gr.pB.y, pA.y, pB.y);

            bool zOverlap = valueInRange(pA.z, gr.pA.z, gr.pB.z) || valueInRange(gr.pA.z, pA.z, pB.z)
                            || valueInRange(pB.z, gr.pA.z, gr.pB.z) || valueInRange(gr.pB.z, pA.z, pB.z);

            if (xOverlap && yOverlap && zOverlap)
                return true;

        }
        return false;
    }

    private void selection(Ray ray)
    {
        RaycastHit hitInfo;
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hitInfo))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    pA = grid.GetNearestPointOnGrid(hitInfo.point) / 2;
                    getSelectedGroup(hitInfo.collider.gameObject);
                    if (!groupSelected)
                    {
                        if (!currentSelection)
                        {
                            currentSelection = Instantiate(selectionBloc, pA, Quaternion.identity);
                            currentSelection.transform.parent = levelObj.transform;
                        }
                        currentSelection.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                        currentSelection.transform.position = pA * 2;
                    }
                    dropButtonChannel.gameObject.transform.parent.gameObject.SetActive(false);
                }
                else if (Input.GetMouseButton(0) && !groupSelected && hitInfo.collider.gameObject.name == "Plane")
                {
                    Vector3 oneVector = new Vector3(1,0,1);
                    pB = grid.GetNearestPointOnGrid(hitInfo.point) / 2;
                    if (isOverlapping())
                        return;

                    Vector3 pa2 = pA, pb2 = pB;
                    getSelectionPointVisual(ref pa2, ref pb2);
                    
                    Vector3 diff = (pb2 - pa2);
                    diff += new Vector3(diff.x >= 0 ? 0.01f : -0.01f, diff.y >= 0 ? 0.01f : -0.01f, diff.z >= 0 ? 0.01f : -0.01f);
                    currentSelection.transform.localScale = diff;
                    currentSelection.transform.position = new Vector3(pa2.x * 2 + ((pb2.x - pa2.x)) - 1, pa2.y * 2 + ((pb2.y - pa2.y)) - 1, pa2.z * 2 + (pb2.z - pa2.z) - 1);

                    interactSelection();
                }
            }
    }

    private void interactSelection()
    {
        GameObject singleObject = arr[(int)pA.x, (int)pA.y, (int)pA.z];
        if (pA == pB && singleObject && (singleObject.transform.Find("buttonInteract") || singleObject.transform.Find("leverInteract")))
        {
            bool alreadyInList = false;
            foreach (Interactable i in interactables.ToList())
            {
                if (!i.gameObject)
                {
                    interactables.Remove(i);
                    break;
                }

                if (i.gameObject.transform.position == singleObject.transform.position)
                    alreadyInList = true;
            }

            if (!alreadyInList)
            {
                Interactable inter = new Interactable();
                inter.gameObject = singleObject;
                inter.channel = 0;
                interactables.Add(inter);
            }

            interactableGameObject = singleObject;
            addButton.SetActive(false);
            popUpGroup.SetActive(false);
            dropButtonChannel.gameObject.transform.parent.gameObject.SetActive(true);
            setInteractableUIValues();
            updateOutline();
        }
        else
        {
            addButton.SetActive(!groupSelected && currentSelection);
            dropButtonChannel.gameObject.transform.parent.gameObject.SetActive(false);
            interactableGameObject = null;
        }
    }

    private void setInteractableUIValues()
    {
        foreach (Interactable i in interactables)
            if (i.gameObject.transform.position == interactableGameObject.transform.position)
                dropButtonChannel.value = i.channel;
    }


    private void getSelectedGroup(GameObject hitObject)
    {
        if (hitObject.name == "SelectionCube")
        {
            groupSelected = hitObject.transform.parent;
            Destroy(currentSelection);
            updateGroupComposition();
        }
        else
        {
            groupSelected = null;
        }

        popUpGroup.SetActive(groupSelected);
        addButton.SetActive(!groupSelected);
        updateOutline();
    }

    private void setPopUpValues()
    {
        var cmp = getComponentInGroups(groupSelected.gameObject);
        dropScript.value = cmp.id;
        dropChannel.value = cmp.channel;
        fieldSpeed.text = cmp.speed.ToString();
        fieldX.text = cmp.pos.x.ToString();
        fieldY.text = cmp.pos.y.ToString();
        fieldZ.text = cmp.pos.z.ToString();

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
        arr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z].transform.parent = levelObj.transform;
        //Applying rotation after instantiation
        arr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z].transform.rotation = currentBloc.transform.rotation;
        
        //Add id
        idArr[(int)gridPosition.x, (int)gridPosition.y, (int)gridPosition.z] = currentId;
        updateGroupComposition();
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
        updateGroupComposition();
    }

    private void ShowCubeNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        if (!currentBloc)
        {
            currentBloc = Instantiate(prefabs[currentId], finalPosition, Quaternion.identity);
            currentBloc.transform.parent = levelObj.transform;
            SetTransparentRecursively(currentBloc.transform.gameObject, 2);
        }
        currentBloc.transform.position = finalPosition;
    }


    private void SetTransparentRecursively(GameObject obj, int layer) {
        obj.layer = layer;
        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer)
        {
            Material[] mats = renderer.materials;
            for (int i = 0; i < renderer.materials.Length; i++)
                mats[i] = transparentMaterial;
            renderer.materials = mats;
        }

        foreach (Transform child in obj.transform) {
            SetTransparentRecursively(child.gameObject, layer);
        }
    }

    public void WriteJson()
    {
        eltCollection = new ElementCollection();
        eltCollection.elements = new List<ElementJson>();

        //Grid
        for (int x = 0; x < lenght; x++)
            for (int y = 0; y < lenght; y++)
                for (int z = 0; z < lenght; z++)
                    if (arr[x, y, z])
                        {
                            ElementJson newElt = new ElementJson();
                            newElt.id = idArr[x, y, z];
                            newElt.position = arr[x, y, z].transform.position;
                            newElt.rotation = arr[x, y, z].transform.rotation;
                            eltCollection.elements.Add(newElt);
                        }

        //Groups
        eltCollection.groups = new List<GroupJson>();
        foreach(Group gr in groups)
        {
            GroupJson newGrp = new GroupJson();
            newGrp.pA = gr.pA;
            newGrp.pB = gr.pB;
            newGrp.component = new ComponentJson();
            newGrp.component.id = gr.id;
            newGrp.component.position = gr.pos;
            newGrp.component.speed = gr.speed;
            newGrp.component.channel = gr.channel;
            eltCollection.groups.Add(newGrp);
        }

        //interactables
        eltCollection.interactables = new List<InteractableJson>();
        foreach(Interactable i in interactables)
        {
            InteractableJson newInter = new InteractableJson();
            newInter.pos = i.gameObject.transform.position;
            newInter.channel = i.channel;
            eltCollection.interactables.Add(newInter);
        }

        string jsonFile = JsonUtility.ToJson(eltCollection);
        File.WriteAllText(Application.dataPath + levelName, jsonFile);
    }

    public void LoadJson()
    {
        //string content = File.ReadAllText(Application.dataPath + levelName);
        //eltCollection = JsonUtility.FromJson<ElementCollection>(content);

        eltCollection = currentLevel.jsonlevel;

        //create the file for playMode to use
        File.WriteAllText(Application.dataPath + levelName, JsonUtility.ToJson(eltCollection));

        for (int x = 0; x < lenght; x++)
            for (int y = 0; y < lenght; y++)
                for (int z = 0; z < lenght; z++)
                    if (arr[x, y, z])
                    {
                        Destroy(arr[x, y, z]);
                        arr[x, y, z] = null;
                    }
        interactables.Clear();

        foreach (Group g in groups)
            Destroy(g.gameObject);
        groups.Clear();

        Destroy(currentSelection);
        popUpGroup.SetActive(false);
        addButton.SetActive(false);
    
        updateOutline();

        //Load grid
        foreach (ElementJson elt in eltCollection.elements)
        {
            int id = (int)elt.id;
            arr[(int)elt.position.x / 2, (int)elt.position.y / 2, (int)elt.position.z / 2] = Instantiate(prefabs[id], elt.position, elt.rotation);
            arr[(int)elt.position.x / 2, (int)elt.position.y / 2, (int)elt.position.z / 2].transform.parent = levelObj.transform;
            idArr[(int)elt.position.x / 2, (int)elt.position.y / 2, (int)elt.position.z / 2] = id;
        }

        //Load groups
        foreach(GroupJson gr in eltCollection.groups)
        {
            Vector3 pa2 = gr.pA, pb2 = gr.pB;

            getSelectionPointVisual(ref pa2, ref pb2);
            
            Vector3 diff = (pb2 - pa2);
            diff += new Vector3(diff.x >= 0 ? 0.01f : -0.01f, diff.y >= 0 ? 0.01f : -0.01f, diff.z >= 0 ? 0.01f : -0.01f);
            selectionBloc.transform.localScale = diff;
            selectionBloc.transform.position = new Vector3(pa2.x * 2 + ((pb2.x - pa2.x)) - 1, pa2.y * 2 + ((pb2.y - pa2.y)) - 1, pa2.z * 2 + (pb2.z - pa2.z) - 1);
            
            //ADD GROUP
            GameObject current = Instantiate(selectionBloc);
            current.transform.parent = groupsObj.transform;
            current.transform.Find("SelectionCube").gameObject.AddComponent(typeof(cakeslice.Outline)).GetComponent<cakeslice.Outline>().color = 3;
            current.transform.Find("SelectionCube").gameObject.AddComponent(typeof(BoxCollider));

            pa2 = gr.pA;
            pb2 = gr.pB;

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

            groupSelected = current.transform;
            
            //Add group element
            Group newGrp = new Group();
            newGrp.gameObject = current;
            newGrp.id = gr.component.id;
            newGrp.pos = gr.component.position;
            newGrp.speed = gr.component.speed;
            newGrp.channel = gr.component.channel;
            groups.Add(newGrp);
            updateOutline();
            setPopUpValues();
        }

        //Load interractables
        foreach(InteractableJson i in eltCollection.interactables)
        {
            Interactable newInter = new Interactable();
            newInter.gameObject =  arr[(int)i.pos.x / 2, (int)i.pos.y / 2, (int)i.pos.z / 2];
            newInter.channel = i.channel;
            interactables.Add(newInter);
        }
    }

    public void LaunchPlayMode()
    {
        WriteJson();
        editor.SetActive(false);
        playMode.SetActive(true);
        //Destroy(GameObject.FindGameObjectWithTag("Player"));
    }

    public void returnToMenu()
    {
        menuEditor.SetActive(true);
		editor.SetActive(false);
        //Destroy(GameObject.FindGameObjectWithTag("Player"));
    }
}