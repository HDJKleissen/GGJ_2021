using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject Menu;
    public GameObject GameUI;

    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInputManager.GetKeyDown("Pause"))
        {
            if (paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        if (GameInputManager.GetKeyDown("Restart"))
        {
            RestartLevel();
        }
    }

    public void Pause()
    {
        Menu.SetActive(true);
        GameUI.SetActive(false);
        Time.timeScale = 0;
        paused = true;
    }

    public void Unpause()
    {
        Menu.SetActive(false);
        GameUI.SetActive(true);
        Time.timeScale = 1;
        paused = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
