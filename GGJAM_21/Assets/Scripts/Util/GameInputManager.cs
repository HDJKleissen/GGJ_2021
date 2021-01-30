using UnityEngine;
using System.Collections.Generic;
using System;

public static class GameInputManager
{
    public static Dictionary<string, KeyCode> keyMapping;
    public static string[] keyMaps = new string[]
    {
        "MoveLeft",
        "MoveRight",
        "Jump",
        "DoubleJump",
        "Dash",
        "BackDash",
        "Right"
    };
    public static KeyCode[] defaults = new KeyCode[]
    {
        KeyCode.A,
        KeyCode.D,
        KeyCode.S,
        KeyCode.S,
        KeyCode.Z,
        KeyCode.X,
        KeyCode.C
    };

    static GameInputManager()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

    public static bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    }

    public static bool GetKey(string keyMap)
    {
        return Input.GetKey(keyMapping[keyMap]);
    }

}