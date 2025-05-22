using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WinGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator WinGame() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ContemporaryMinigame2");
        SceneManager.UnloadScene("GameScene");
    }
}
