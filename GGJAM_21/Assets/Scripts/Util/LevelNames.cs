using System.Collections.Generic;

using UnityEngine.SceneManagement;

public static class LevelNames
{

    static int NON_LEVEL_SCENES_IN_BUILD_SETTINGS = 3;

    public static Dictionary<int, string> LevelNumToLevelName = new Dictionary<int, string>();

    public static Dictionary<string, int> LevelNameToLevelNum = new Dictionary<string, int>();

    static LevelNames()
    {
        // All scenes in build minus Main Menu, End Screen, Sandbox.
        // UPDATE THIS IF YOU ADD NON-LEVEL SCENES TO THE BUILD SETTINGS OR SHIT GETS BORKED
        // above message probably means this is a terrible system but oh well ¯\_(ツ)_/¯
        int scenesAmount = SceneManager.sceneCountInBuildSettings - NON_LEVEL_SCENES_IN_BUILD_SETTINGS;

        for(int i = 0; i < scenesAmount; i++)
        {
            LevelNumToLevelName.Add(i, "Level_" + i);
            LevelNameToLevelNum.Add("Level_" + i, i);
        }

        // Sandbox gets -2 so it goes to the main menu instead of level 0 
        LevelNumToLevelName.Add(-2, "Sandbox");
        LevelNameToLevelNum.Add("Sandbox", -2);
    }
}