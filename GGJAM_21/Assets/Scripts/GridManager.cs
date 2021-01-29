using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float cellSize = 64.0f;
    public Transform tilePrefab;
    public TileSet tileSet;

    public Color gridGizmoColor = Color.white;

    private float gizmoGridSize = 3200.0f;
    /* Draw grid lines in the editor */
    private void OnDrawGizmos()
    {
        Vector3 pos = Camera.current.transform.position;
        Gizmos.color = gridGizmoColor;

        for(float y = pos.y - gizmoGridSize; y < pos.y + gizmoGridSize; y+= cellSize)
        {
            Gizmos.DrawLine(new Vector3(-10000.0f, Mathf.Floor(y / cellSize) * cellSize, 0.0f),
                            new Vector3(10000.0f, Mathf.Floor(y / cellSize) * cellSize, 0.0f));
        }

        for(float x = pos.x - gizmoGridSize; x < pos.x + gizmoGridSize; x+= cellSize)
        {
            Gizmos.DrawLine(new Vector3(Mathf.Floor(x / cellSize) * cellSize, -10000.0f, 0.0f),
                new Vector3(Mathf.Floor(x / cellSize) * cellSize, 10000.0f, 0.0f));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
