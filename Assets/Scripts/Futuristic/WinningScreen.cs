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
		yield return new WaitForSeconds(5f);
		SceneManager.LoadScene(nextSceneName);
	}
}
