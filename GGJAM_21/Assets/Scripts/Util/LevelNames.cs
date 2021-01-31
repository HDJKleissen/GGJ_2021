using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class LevelNames
{
    public static Dictionary<int, string> LevelNumToLevelName = new Dictionary<int, string>()
    {
        { 0, "Level_0" },
        { 1, "Level_1" },
        //{ 2, "Level_2" },
        //{ 3, "Level_3" },
        //{ 4, "Level_4" },
        //{ 5, "Level_5" },
    };

    public static Dictionary<string, int> LevelNameToLevelNum = new Dictionary<string, int>()
    {
        { "Level_0", 0 },
        { "Level_1", 1 },
        //{ "Level_2", 2 },
        //{ "Level_3", 3 },
        //{ "Level_4", 4 },
        //{ "Level_5", 5 },
    };
}