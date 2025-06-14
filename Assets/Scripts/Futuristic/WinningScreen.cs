using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinningScreen : MonoBehaviour
{
	public string nextSceneName;

	public void ShowAndProceed()
	{
		StartCoroutine(ShowWinningScreenAndLoadNext());
	}

	private IEnumerator ShowWinningScreenAndLoadNext()
	{
		yield return new WaitForSeconds(3f); // wait for 3 seconds to show the winning screen
		SceneManager.LoadScene(nextSceneName); // load the next scene
	}
}
