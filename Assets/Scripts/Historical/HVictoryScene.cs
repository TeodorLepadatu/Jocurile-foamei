using UnityEngine;
using UnityEngine.SceneManagement;

public class HVictoryScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private System.Collections.IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("CMinigame1");
    }
}
