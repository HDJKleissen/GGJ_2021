using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject SmallUIButtons;

    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        Unpause();
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
        PauseMenu.SetActive(true);
        SmallUIButtons.SetActive(false);
        Time.timeScale = 0;
        paused = true;
    }

    public void Unpause()
    {
        PauseMenu.SetActive(false);
        SmallUIButtons.SetActive(true);
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
