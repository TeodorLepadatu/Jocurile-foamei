using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;
        
        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "TowerDefence") {
            SceneManager.LoadScene("MainScene1");
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        Scene activeScene = SceneManager.GetActiveScene();

        //SceneManager.UnloadScene(activeScene);
        SceneManager.LoadScene("MainMenu");
        isPaused = false;
    }
    
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
