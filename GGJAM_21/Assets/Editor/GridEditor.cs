using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    private GridManager grid;
    private int oldIndex = 0;
    private void OnEnable()
    {
        oldIndex = 0;
        grid = (GridManager)target;
    }

    [MenuItem("Assets/Create/TileSet")]
    static void CreateTileSet()
    {
        ScriptableObject asset = ScriptableObject.CreateInstance<TileSet>();
        
        //string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        //Debug.Log(path);

        var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/TileSet/TileSet.asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        asset.hideFlags = HideFlags.DontSave;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUILayout.HelpBox("Once the GridManager Object/attached Script is selected: Leftclick in the Scene to place tile, Right to remove a Tile. GL!", MessageType.Info);

        GUILayout.BeginHorizontal();
        GUILayout.Label("GridCell Size");
        grid.cellSize = EditorGUILayout.Slider(grid.cellSize, 1.0f, 128.0f, null);
        GUILayout.EndHorizontal();


        if(GUILayout.Button("Open Grid Window"))
        {
            GridWindow window = EditorWindow.GetWindow<GridWindow>();
            window.init();
        }

        //Tile Prefab
        EditorGUI.BeginChangeCheck();
        Transform newTilePrefab = (Transform)EditorGUILayout.ObjectField("Tile Prefab", grid.tilePrefab, typeof(Transform), false);
        if (EditorGUI.EndChangeCheck())
        {
            grid.tilePrefab = newTilePrefab;
            Undo.RecordObject(target, "Grid Changed");
        }

        //Tile Map
        EditorGUI.BeginChangeCheck();
        TileSet newTileSet = (TileSet)EditorGUILayout.ObjectField("Tileset", grid.tileSet, typeof(TileSet), false);
        if (EditorGUI.EndChangeCheck())
        {
            grid.tileSet = newTileSet;
            Undo.RecordObject(target, "Grid Changed");
        }


        if(grid.tileSet != null)
        {
            EditorGUI.BeginChangeCheck();
            string[] names = new string[grid.tileSet.prefabs.Length];
            int[] values = new int[names.Length];

            for (int i = 0; i < names.Length; ++i)
            {
                names[i] = grid.tileSet.prefabs[i] != null ? grid.tileSet.prefabs[i].name : "";
                values[i] = i;
            }

            int currentSelectedIndex = EditorGUILayout.IntPopup("Select Tile", oldIndex, names, values);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Grid Tile Selection Changed!");
                if(oldIndex != currentSelectedIndex)
                {
                    oldIndex = currentSelectedIndex;
                    grid.tilePrefab = grid.tileSet.prefabs[currentSelectedIndex];

                    float size = grid.tilePrefab.GetComponent<Renderer>().bounds.size.x;

                    if(grid.cellSize != size)
                    {
                        Debug.LogWarning("GridSize changed because of TileSelection!");
                        grid.cellSize = size;
                    }
                }
            }
        }
    }

    /* Onclick when Grid is Selected */
    private void OnSceneGUI()
    {
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        Event e = Event.current;
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight, 0));
        Vector3 mousePosition = ray.origin;

        //place tile
        if(e.isMouse && e.type == EventType.MouseDown && e.button == 0)
        {
            GUIUtility.hotControl = controlId;
            e.Use();

            GameObject tile;
            Transform prefab = grid.tilePrefab;

            if (prefab)
            {
                Undo.IncrementCurrentGroup();
                tile = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);
                Vector3 gridAlignedPosition = new Vector3(Mathf.Floor(mousePosition.x / grid.cellSize) * grid.cellSize + grid.cellSize / 2.0f,
                                              Mathf.Floor(mousePosition.y / grid.cellSize) * grid.cellSize + grid.cellSize / 2.0f, 0.0f);
                tile.transform.position = gridAlignedPosition;
                Debug.Log("Placing Tile at: " + tile.transform.position + " Mouse Pos: " + mousePosition);
                tile.transform.parent = grid.transform;
                Undo.RegisterCreatedObjectUndo(tile, "Create " + tile.name);
            }
        }

        //right click remove tile
        if (e.isMouse & e.type == EventType.ContextClick)
        {
            GUIUtility.hotControl = controlId;
            e.Use();
            Vector3 aligned = new Vector3(Mathf.Floor(mousePosition.x / grid.cellSize) * grid.cellSize + grid.cellSize / 2.0f, 
                                          Mathf.Floor(mousePosition.y / grid.cellSize) * grid.cellSize + grid.cellSize / 2.0f, 0.0f);
            Transform transform = GetTransformFromPosition(aligned);
            if (transform != null)
            {
                DestroyImmediate(transform.gameObject);
            }
        }

        //unclick
        if (e.isMouse && e.type == EventType.MouseUp)
        {
            GUIUtility.hotControl = 0;
        }
    }

    Transform GetTransformFromPosition(Vector3 aligned)
    {
        int i = 0;
        while (i < grid.transform.childCount)
        {
            Transform transform = grid.transform.GetChild(i);
            if (transform.position == aligned)
            {
                return transform;
            }

            i++;
        }
        return null;
    }

}
