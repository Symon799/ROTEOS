using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float size = 2f;
    public int lenght = 40;
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
        {
            for (float z = 0; z < lenght; z += size)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));

                var pointLine = GetNearestPointOnGrid(new Vector3(x, 0f, z));

                var nextPoint1 = GetNearestPointOnGrid(new Vector3(x + 2, 0f, z));
                var nextPoint2 = GetNearestPointOnGrid(new Vector3(x, 0f, z + 2));

                Gizmos.DrawSphere(point, 0.2f);

                //Gizmos.DrawLine(pointLine + new Vector3(-1,-1,-1), nextPoint1 + new Vector3(-1,-1,-1));
                //Gizmos.DrawLine(pointLine + new Vector3(-1,-1,-1), nextPoint2 + new Vector3(-1,-1,-1));
            }
                
        }
    }


}