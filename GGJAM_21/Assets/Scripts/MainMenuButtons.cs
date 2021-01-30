using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{

    public List<Button> menuButtons = new List<Button>();

    private void Awake()
    {
        menuButtons = new List<Button>(GetComponentsInChildren<Button>());
    }

    public void StartGame()
    {
        Debug.Log("Start click");
        SceneManager.LoadScene("Level0");
    }

    public void QuitGame()
    {
        Debug.Log("Quit click");
        Application.Quit();
    }
}
