using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    private GridManager grid;

    private void OnEnable()
    {
        grid = (GridManager)target;
    }

    [MenuItem("Assets/Create/TileSet")]
    static void CreateTileSet()
    {
        ScriptableObject asset = ScriptableObject.CreateInstance<TileSet>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        Debug.Log(path);

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

    }

}
