using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // toggle pause menu
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
        Scene activeScene = SceneManager.GetActiveScene();
        
        SceneManager.LoadScene("MainScene1");
        Time.timeScale = 1f;
        isPaused = false;
        CurrencyHolder.setCurrency(10); // Reset coins
    }


    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // freeze time
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        Scene activeScene = SceneManager.GetActiveScene();

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
