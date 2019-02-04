using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public Vector2 tileSize;
    public Vector2 gridTileSize;
    public List<Vector3> positions = new List<Vector3>();
    // Start is called before the first frame update



    private void OnDrawGizmos()
    {
        foreach (Vector3 pos in positions)
        {
            Gizmos.DrawCube(pos, gizSize);
        }
    }
}
