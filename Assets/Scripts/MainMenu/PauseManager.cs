using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject gameWrapper;
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
        gameWrapper.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        SceneManager.UnloadScene(activeScene);
        SceneManager.LoadScene("MainScene1");
        Time.timeScale = 1f;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameWrapper.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        Scene activeScene = SceneManager.GetActiveScene();

        SceneManager.UnloadScene(activeScene);
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
