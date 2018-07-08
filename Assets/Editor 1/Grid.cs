using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float size = 2f;
    public int lenght = 40;

    private void Awake()
    {
        drawGrid();
    }
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = 0; x < lenght; x += size)
            for (float z = 0; z < lenght; z += size)
                Gizmos.DrawSphere(GetNearestPointOnGrid(new Vector3(x, 0f, z)), 0.2f);
    }

    void drawGrid()
    {
        var point = GetNearestPointOnGrid(new Vector3(0, 0, 0)) - new Vector3(0.5f, 0.5f, 0.5f);
        for (int i = 0; i < (lenght / 2) + 1; i++)
            DrawLine(point + new Vector3(i ,0, 0), point + new Vector3(i , 0, 40));

        for (int j = 0; j < (lenght / 2) + 1; j++)
            DrawLine(point + new Vector3(0 ,0, j), point + new Vector3(40, 0, j));

    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject();
        myLine.transform.parent = transform.GetChild(0).gameObject.transform;
        myLine.transform.position = start;
        LineRenderer lineRenderer = myLine.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.receiveShadows = false;
        lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        //Color32 color32 = new Color32(255, 255, 255, 255);
        lineRenderer.startColor = Color.green;
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    
}