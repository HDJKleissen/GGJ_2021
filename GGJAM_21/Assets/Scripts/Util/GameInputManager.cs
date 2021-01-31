using UnityEngine;
using System.Collections.Generic;
using System;

public static class GameInputManager
{
    public static Dictionary<string, KeyCode> keyMapping = new Dictionary<string, KeyCode>()
    {
        { "MoveLeft", KeyCode.A },
        { "MoveRight", KeyCode.D },
        { "Jump", KeyCode.W },
        { "DoubleJump", KeyCode.W },
        { "Dash", KeyCode.Z },
        { "BackDash",KeyCode.X },
        { "Right", KeyCode.C },
        { "Pause", KeyCode.P },
        { "Restart", KeyCode.R },
        { "Crouch", KeyCode.S }
    };

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