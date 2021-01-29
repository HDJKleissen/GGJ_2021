using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridWindow : EditorWindow
{
    GridManager grid;

    public void init()
    {
        grid = (GridManager)FindObjectOfType(typeof(Grid));
    }

    private void OnGUI()
    {
        grid.gridGizmoColor = EditorGUILayout.ColorField(grid.gridGizmoColor, GUILayout.Width(100));
    }
}

