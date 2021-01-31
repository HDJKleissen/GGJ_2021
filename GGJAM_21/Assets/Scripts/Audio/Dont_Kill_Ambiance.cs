using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dont_Kill_Ambiance : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ambiance");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
