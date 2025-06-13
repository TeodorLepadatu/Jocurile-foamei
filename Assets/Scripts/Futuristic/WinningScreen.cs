using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinningScreen : MonoBehaviour
{
	public string nextSceneName;

	void Start()
	{
		StartCoroutine(ShowWinningScreenAndLoadNext());
	}

	IEnumerator ShowWinningScreenAndLoadNext()
	{
		yield return new WaitForSeconds(5f);
		SceneManager.LoadScene(nextSceneName);
	}
}
